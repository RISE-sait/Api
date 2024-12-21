package MembershipsPlans

import (
	db "api/db/sqlc"
	"encoding/json"
	"net/http"

	"github.com/go-chi/chi"
	"github.com/google/uuid"
)

type MembershipPlansController struct {
	queries *db.Queries
}

func NewController(queries *db.Queries) *MembershipPlansController {
	return &MembershipPlansController{queries: queries}
}

func (c *MembershipPlansController) CreateMembershipPlan(w http.ResponseWriter, r *http.Request) {
	var params db.CreateMembershipPlanParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	plan, err := c.queries.CreateMembershipPlan(r.Context(), params)
	if err != nil {
		http.Error(w, "Failed to create membership plan: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(plan); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *MembershipPlansController) GetMembershipPlans(w http.ResponseWriter, r *http.Request) {
	membershipIDStr := r.URL.Query().Get("membership_id")
	planIDStr := r.URL.Query().Get("plan_id")

	var membershipID, planID uuid.UUID = uuid.Nil, uuid.Nil
	if membershipIDStr != "" {
		id, err := uuid.Parse(membershipIDStr)
		if err != nil {
			http.Error(w, "Invalid membership ID format", http.StatusBadRequest)
			return
		}
		membershipID = id
	}

	if planIDStr != "" {
		id, err := uuid.Parse(planIDStr)
		if err != nil {
			http.Error(w, "Invalid plan ID format", http.StatusBadRequest)
			return
		}
		planID = id
	}

	plans, err := c.queries.GetMembershipPlans(r.Context(), db.GetMembershipPlansParams{
		Column1: membershipID,
		Column2: planID,
	})
	if err != nil {
		http.Error(w, "Failed to get membership plans: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(plans); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *MembershipPlansController) UpdateMembershipPlan(w http.ResponseWriter, r *http.Request) {
	var params db.UpdateMembershipPlanParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	if err := c.queries.UpdateMembershipPlan(r.Context(), params); err != nil {
		http.Error(w, "Failed to update membership plan: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (c *MembershipPlansController) DeleteMembershipPlan(w http.ResponseWriter, r *http.Request) {
	membershipIDStr := chi.URLParam(r, "membership_id")
	planIDStr := chi.URLParam(r, "id")

	membershipID, err := uuid.Parse(membershipIDStr)
	if err != nil {
		http.Error(w, "Invalid membership ID format", http.StatusBadRequest)
		return
	}

	planID, err := uuid.Parse(planIDStr)
	if err != nil {
		http.Error(w, "Invalid plan ID format", http.StatusBadRequest)
		return
	}

	params := db.DeleteMembershipPlanParams{
		MembershipID: membershipID,
		ID:           planID,
	}

	if err := c.queries.DeleteMembershipPlan(r.Context(), params); err != nil {
		http.Error(w, "Failed to delete membership plan: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
