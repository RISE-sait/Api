package memberships

import (
	db "api/db/sqlc"
	"context"
	"database/sql"
	"encoding/json"
	"net/http"

	"github.com/go-chi/chi"
	"github.com/google/uuid"
)

type MembershipsController struct {
	queries *db.Queries
}

func NewController(queries *db.Queries) *MembershipsController {
	return &MembershipsController{queries: queries}
}

func (c *MembershipsController) CreateMembership(w http.ResponseWriter, r *http.Request) {
	var params db.CreateMembershipParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	membership, err := c.queries.CreateMembership(context.Background(), params)
	if err != nil {
		http.Error(w, "Failed to create membership: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	if err := json.NewEncoder(w).Encode(membership); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *MembershipsController) GetMembershipById(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	membership, err := c.queries.GetMembershipById(context.Background(), id)
	if err != nil {
		if err == sql.ErrNoRows {
			http.Error(w, "Membership not found", http.StatusNotFound)
		} else {
			http.Error(w, "Failed to get membership: "+err.Error(), http.StatusInternalServerError)
		}
		return
	}

	w.Header().Set("Content-Type", "application/json")
	if err := json.NewEncoder(w).Encode(membership); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *MembershipsController) GetAllMemberships(w http.ResponseWriter, r *http.Request) {
	memberships, err := c.queries.GetAllMemberships(context.Background())
	if err != nil {
		http.Error(w, "Failed to get memberships: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	if err := json.NewEncoder(w).Encode(memberships); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *MembershipsController) UpdateMembership(w http.ResponseWriter, r *http.Request) {
	var params db.UpdateMembershipParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	if err := c.queries.UpdateMembership(context.Background(), params); err != nil {
		http.Error(w, "Failed to update membership: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (c *MembershipsController) DeleteMembership(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	if err := c.queries.DeleteMembership(context.Background(), id); err != nil {
		http.Error(w, "Failed to delete membership: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
