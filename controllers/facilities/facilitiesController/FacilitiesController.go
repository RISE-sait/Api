package facilitiesController

import (
	db "api/db/sqlc"
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"net/http"

	"github.com/google/uuid"
)

type FacilitiesController struct {
	queries *db.Queries
}

func NewController(queries *db.Queries) *FacilitiesController {
	return &FacilitiesController{queries: queries}
}

func (c *FacilitiesController) CreateFacility(w http.ResponseWriter, r *http.Request) {
	var req db.CreateFacilityParams
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, "Invalid request payload", http.StatusBadRequest)
		return
	}

	facility, err := c.queries.CreateFacility(context.Background(), req)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	if err := json.NewEncoder(w).Encode(facility); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
}

// GetFacility handles retrieving a single facility by its ID
func (c *FacilitiesController) GetFacility(w http.ResponseWriter, r *http.Request) {
	id := r.URL.Query().Get("id")

	parsedID, err := uuid.Parse(id)
	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	facility, err := c.queries.GetFacilityById(context.Background(), parsedID)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			http.Error(w, "Facility not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	if err := json.NewEncoder(w).Encode(facility); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
}

// GetAllFacilities handles retrieving all facilities
func (c *FacilitiesController) GetAllFacilities(w http.ResponseWriter, _ *http.Request) {
	facilities, err := c.queries.GetAllFacilities(context.Background())
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	if facilities == nil {
		facilities = []db.Facility{}
	}

	if err := json.NewEncoder(w).Encode(facilities); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
}

// UpdateFacility handles updating an existing facility
func (c *FacilitiesController) UpdateFacility(w http.ResponseWriter, r *http.Request) {
	id := r.URL.Query().Get("id")

	parsedID, err := uuid.Parse(id)
	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	var req db.UpdateFacilityParams
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, "Invalid request payload", http.StatusBadRequest)
		return
	}
	req.ID = parsedID

	err = c.queries.UpdateFacility(context.Background(), req)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			http.Error(w, "Facility not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	w.WriteHeader(http.StatusOK)
	_, _ = w.Write([]byte("Facility updated successfully"))
}

// DeleteFacility handles deleting a facility by its ID
func (c *FacilitiesController) DeleteFacility(w http.ResponseWriter, r *http.Request) {
	id := r.URL.Query().Get("id")

	parsedID, err := uuid.Parse(id)
	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	err = c.queries.DeleteFacility(context.Background(), parsedID)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			http.Error(w, "Facility not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	w.WriteHeader(http.StatusOK)
	_, _ = w.Write([]byte("Facility deleted successfully"))
}
