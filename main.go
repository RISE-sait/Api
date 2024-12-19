package main

import (
	"api/config"
	"api/controllers/customers"
	"api/controllers/facilities/facilitiesController"
	facilitiesTypesController "api/controllers/facilities/facilitiesTypes"
	"api/controllers/oauth/callback"
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
	dbConn := config.GetDBConnection()

	queries := db.New(dbConn)
	facilitiesCtrl := facilitiesController.NewController(queries)
	facilitiesTypesCtrl := facilitiesTypesController.NewController(queries)
	schedulesCtrl := schedules.NewController(queries)

	hubSpotService := services.GetHubSpotService()
	customersCtrl := customers.NewController(hubSpotService)

	router := chi.NewRouter()

	router.Use(middleware.Logger)
	router.Use(middlewares.SetJSONContentType)

	registerFacilitiesRoutes(router, facilitiesCtrl)
	registerFacilitiesTypesRoutes(router, facilitiesTypesCtrl)
	registerCustomersRoutes(router, customersCtrl)
	registerSchedulesRoutes(router, schedulesCtrl)
	registerAuthRoutes(router)

	// Start the server
	log.Println("Server started at :8080")
	log.Fatal(http.ListenAndServe(":8080", router))
}

func registerFacilitiesRoutes(router *chi.Mux, controller *facilitiesController.FacilitiesController) {

	router.Route("/api/facilities", func(r chi.Router) {
		r.Get("/", controller.GetAllFacilities)
		r.Post("/", controller.CreateFacility)
		r.Get("/{id}", controller.GetFacility)
	})
}

func registerFacilitiesTypesRoutes(router *chi.Mux, controller *facilitiesTypesController.FacilityTypesController) {

	router.Route("/api/facilities/types", func(r chi.Router) {
		r.Get("/", controller.GetAllFacilityTypes)
		r.Post("/", controller.CreateFacilityType)
		r.Get("/{id}", controller.GetFacilityTypeByID)
	})
}

func registerCustomersRoutes(router *chi.Mux, controller *customers.CustomerController) {

	router.Route("/api/customers", func(r chi.Router) {
		r.Get("/", controller.GetCustomers)
	})
}

func registerAuthRoutes(router *chi.Mux) {

	router.Route("/oauth/callback", func(r chi.Router) {
		r.Get("/", callback.HandleOAuthCallback)
	})
}

func registerSchedulesRoutes(router *chi.Mux, controller *schedules.SchedulesController) {

	router.Route("/api/schedules", func(r chi.Router) {
		r.Get("/", controller.GetAllSchedules)
		r.Post("/", controller.CreateSchedule)
		r.Get("/{id}", controller.GetScheduleByID)
		r.Put("/{id}", controller.UpdateSchedule)
		r.Delete("/{id}", controller.DeleteSchedule)
	})
}
