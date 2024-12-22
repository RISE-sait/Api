package main

import (
	"api/config"
	"api/controllers/courses"
	"api/controllers/customers"
	"api/controllers/facilities/facilitiesController"
	facilitiesTypesController "api/controllers/facilities/facilitiesTypes"
	"api/controllers/memberships"
	MembershipsPlans "api/controllers/memberships/plans"
	"api/controllers/schedules"
	db "api/db/sqlc"
	"api/middlewares"
	"api/services"
	"log"
	"net/http"

	"github.com/go-chi/chi"
	"github.com/go-chi/chi/middleware"
	_ "github.com/lib/pq"
)

func main() {

	// Build the connection string
	queries, hubSpotService := initDependencies()

	router := chi.NewRouter()

	router.Use(middleware.Logger)
	router.Use(middlewares.SetJSONContentType)

	registerRoutes(router, queries, hubSpotService)

	// Start the server
	log.Println("Server started at :8080")
	log.Fatal(http.ListenAndServe(":8080", router))
}

func initDependencies() (*db.Queries, *services.HubSpotService) {
	// Database connection
	dbConn := config.GetDBConnection()
	queries := db.New(dbConn)

	// HubSpot service
	hubSpotService := services.GetHubSpotService()

	return queries, hubSpotService
}

func registerRoutes(router *chi.Mux, queries *db.Queries, hubSpotService *services.HubSpotService) {
	facilitiesCtrl := facilitiesController.NewController(queries)
	facilitiesTypesCtrl := facilitiesTypesController.NewController(queries)
	schedulesCtrl := schedules.NewController(queries)
	membershipsCtrl := memberships.NewController(queries)
	coursesCtrl := courses.NewController(queries)
	membershipsPlansCtrl := MembershipsPlans.NewController(queries)

	customersCtrl := customers.NewController(hubSpotService)

	apiRoutes := []struct {
		path   string
		ctrl   interface{}
		config func(chi.Router)
	}{
		{"/facilities", facilitiesCtrl, func(r chi.Router) {
			ctrl := facilitiesCtrl
			r.Get("/", ctrl.GetAllFacilities)
			r.Post("/", ctrl.CreateFacility)
			r.Get("/{id}", ctrl.GetFacility)
		}},
		{"/facilities/types", facilitiesTypesCtrl, func(r chi.Router) {
			ctrl := facilitiesTypesCtrl
			r.Get("/", ctrl.GetAllFacilityTypes)
			r.Post("/", ctrl.CreateFacilityType)
			r.Get("/{id}", ctrl.GetFacilityTypeByID)
		}},
		{"/customers", customersCtrl, func(r chi.Router) {
			ctrl := customersCtrl
			r.Get("/", ctrl.GetCustomers)
		}},
		{"/memberships", membershipsCtrl, func(r chi.Router) {
			ctrl := membershipsCtrl
			r.Get("/", ctrl.GetAllMemberships)
			r.Get("/{id}", ctrl.GetMembershipById)
			r.Post("/", ctrl.CreateMembership)
			r.Put("/{id}", ctrl.UpdateMembership)
			r.Delete("/{id}", ctrl.DeleteMembership)
		}},
		{"/memberships/plans", membershipsPlansCtrl, func(r chi.Router) {
			ctrl := membershipsPlansCtrl
			r.Get("/", ctrl.GetMembershipPlans)
			r.Post("/", ctrl.CreateMembershipPlan)
		}},
		{"/courses", coursesCtrl, func(r chi.Router) {
			ctrl := coursesCtrl
			r.Get("/", ctrl.GetAllCourses)
			r.Get("/{id}", ctrl.GetCourseById)
			r.Post("/", ctrl.CreateCourse)
			r.Put("/{id}", ctrl.UpdateCourse)
			r.Delete("/{id}", ctrl.DeleteCourse)
		}},
		{"/schedules", schedulesCtrl, func(r chi.Router) {
			ctrl := schedulesCtrl
			r.Get("/", ctrl.GetAllSchedules)
			r.Post("/", ctrl.CreateSchedule)
			r.Get("/{id}", ctrl.GetScheduleByID)
			r.Put("/{id}", ctrl.UpdateSchedule)
			r.Delete("/{id}", ctrl.DeleteSchedule)
		}},
	}

	// Register routes under the "/api" base path
	router.Route("/api", func(api chi.Router) {
		for _, route := range apiRoutes {
			api.Route(route.path, route.config)
		}
	})
}
