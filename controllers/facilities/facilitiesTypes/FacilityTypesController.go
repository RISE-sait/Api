package facilitiesTypesController

import (
	db "api/db/sqlc"
	"database/sql"
	"encoding/json"
	"errors"
	"net/http"

	"github.com/go-chi/chi"
	"github.com/google/uuid"
)

type FacilityTypesController struct {
	queries *db.Queries
}

func NewController(queries *db.Queries) *FacilityTypesController {
	return &FacilityTypesController{queries: queries}
}

func (ctrl *FacilityTypesController) GetFacilityTypeByID(w http.ResponseWriter, r *http.Request) {

	idStr := chi.URLParam(r, "id")

	parsedID, err := uuid.Parse(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	facilityType, err := ctrl.queries.GetFacilityTypeById(r.Context(), parsedID)
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			http.Error(w, "Facility type not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	err = json.NewEncoder(w).Encode(facilityType)
	if err != nil {
		return
	}
}

func (ctrl *FacilityTypesController) GetAllFacilityTypes(w http.ResponseWriter, r *http.Request) {
	facilityTypes, err := ctrl.queries.GetAllFacilityTypes(r.Context())
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	if facilityTypes == nil {
		facilityTypes = []db.FacilityType{}
	}

	err = json.NewEncoder(w).Encode(facilityTypes)
	if err != nil {
		return
	}
}

func (ctrl *FacilityTypesController) CreateFacilityType(w http.ResponseWriter, r *http.Request) {
	var request struct {
		Name string `json:"name"`
	}

	if err := json.NewDecoder(r.Body).Decode(&request); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	if request.Name == "" {
		http.Error(w, "Facility type name is required", http.StatusBadRequest)
		return
	}

	facilityType, err := ctrl.queries.CreateFacilityType(r.Context(), request.Name)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	err = json.NewEncoder(w).Encode(facilityType)
	if err != nil {
		return
	}
}

func (ctrl *FacilityTypesController) UpdateFacilityType(w http.ResponseWriter, r *http.Request) {

	idStr := chi.URLParam(r, "id")

	parsedID, err := uuid.Parse(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	// Decode and validate the request body
	var request struct {
		Name string `json:"name"`
	}

	if err := json.NewDecoder(r.Body).Decode(&request); err != nil {
		http.Error(w, "Invalid request format", http.StatusBadRequest)
		return
	}

	if request.Name == "" {
		http.Error(w, "Facility type name is required", http.StatusBadRequest)
		return
	}

	// Call the database query to update the facility type
	err = ctrl.queries.UpdateFacilityType(r.Context(), db.UpdateFacilityTypeParams{
		ID:   parsedID,
		Name: request.Name,
	})
	if err != nil {
		if errors.Is(err, sql.ErrNoRows) {
			http.Error(w, "Facility type not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	// Return a success response
	w.WriteHeader(http.StatusOK)
	_, err = w.Write([]byte("Facility type updated successfully"))
	if err != nil {
		return
	}
}
