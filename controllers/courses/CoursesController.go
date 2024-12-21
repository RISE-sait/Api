package courses

import (
	db "api/db/sqlc"
	"database/sql"
	"encoding/json"
	"net/http"

	"github.com/go-chi/chi"
	"github.com/google/uuid"
)

type CoursesController struct {
	queries *db.Queries
}

func NewController(queries *db.Queries) *CoursesController {
	return &CoursesController{queries: queries}
}

func (c *CoursesController) CreateCourse(w http.ResponseWriter, r *http.Request) {
	var params db.CreateCourseParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	course, err := c.queries.CreateCourse(r.Context(), params)
	if err != nil {
		http.Error(w, "Failed to create course: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(course); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *CoursesController) GetCourseById(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")

	parsedID, err := uuid.Parse(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	course, err := c.queries.GetCourseById(r.Context(), parsedID)
	if err != nil {
		if err == sql.ErrNoRows {
			http.Error(w, "Course not found", http.StatusNotFound)
		} else {
			http.Error(w, "Failed to get course: "+err.Error(), http.StatusInternalServerError)
		}
		return
	}

	if err := json.NewEncoder(w).Encode(course); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *CoursesController) GetAllCourses(w http.ResponseWriter, r *http.Request) {
	courses, err := c.queries.GetAllCourses(r.Context())
	if err != nil {
		http.Error(w, "Failed to get courses: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(courses); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func (c *CoursesController) UpdateCourse(w http.ResponseWriter, r *http.Request) {
	var params db.UpdateCourseParams
	if err := json.NewDecoder(r.Body).Decode(&params); err != nil {
		http.Error(w, "Invalid input: "+err.Error(), http.StatusBadRequest)
		return
	}

	if err := c.queries.UpdateCourse(r.Context(), params); err != nil {
		http.Error(w, "Failed to update course: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (c *CoursesController) DeleteCourse(w http.ResponseWriter, r *http.Request) {
	idStr := chi.URLParam(r, "id")

	parsedID, err := uuid.Parse(idStr)

	if err != nil {
		http.Error(w, "Invalid ID format", http.StatusBadRequest)
		return
	}

	if err := c.queries.DeleteCourse(r.Context(), parsedID); err != nil {
		http.Error(w, "Failed to delete course: "+err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
