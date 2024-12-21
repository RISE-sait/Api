package schedules

import (
	db "api/db/sqlc"
	"database/sql"
	"encoding/json"
	"net/http"
	"strconv"

	"github.com/go-chi/chi"
)

// SchedulesController provides HTTP handlers for managing schedules.
type SchedulesController struct {
	queries *db.Queries
}

// NewController creates a new instance of SchedulesController.
func NewController(queries *db.Queries) *SchedulesController {
	return &SchedulesController{queries: queries}
}

// GetAllSchedules retrieves all schedules from the database.
func (c *SchedulesController) GetAllSchedules(w http.ResponseWriter, r *http.Request) {
	schedules, err := c.queries.GetAllSchedules(r.Context())
	if err != nil {
		http.Error(w, "Failed to fetch schedules: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(schedules); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

// GetScheduleByID retrieves a single schedule by its ID.
func (c *SchedulesController) GetScheduleByID(w http.ResponseWriter, r *http.Request) {

	idStr := chi.URLParam(r, "id")

	parsedID, err := strconv.Atoi(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}
	schedule, err := c.queries.GetScheduleById(r.Context(), int32(parsedID))

	if err != nil {
		if err == sql.ErrNoRows {
			http.Error(w, "Schedule not found", http.StatusNotFound)
		} else {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
		return
	}

	if err := json.NewEncoder(w).Encode(schedule); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

// CreateSchedule creates a new schedule.
func (c *SchedulesController) CreateSchedule(w http.ResponseWriter, r *http.Request) {
	var newSchedule db.CreateScheduleParams
	if err := json.NewDecoder(r.Body).Decode(&newSchedule); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	createdSchedule, err := c.queries.CreateSchedule(r.Context(), newSchedule)
	if err != nil {
		http.Error(w, "Failed to create schedule: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(createdSchedule); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

// UpdateSchedule updates an existing schedule.
func (c *SchedulesController) UpdateSchedule(w http.ResponseWriter, r *http.Request) {
	var schedule db.UpdateScheduleParams
	if err := json.NewDecoder(r.Body).Decode(&schedule); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	if err := c.queries.UpdateSchedule(r.Context(), schedule); err != nil {
		http.Error(w, "Failed to update schedule: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

// DeleteSchedule deletes a schedule by its ID.
func (c *SchedulesController) DeleteSchedule(w http.ResponseWriter, r *http.Request) {

	idStr := chi.URLParam(r, "id")

	parsedID, err := strconv.Atoi(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	deletedCount, err := c.queries.DeleteSchedule(r.Context(), int32(parsedID))

	if deletedCount == 0 {
		http.Error(w, "Schedule not found", http.StatusNotFound)
		return
	}

	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
