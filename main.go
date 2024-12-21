package main

import (
	"api/config"
	"api/controllers/courses"
	"api/controllers/customers"
	"api/controllers/facilities/facilitiesController"
	facilitiesTypesController "api/controllers/facilities/facilitiesTypes"
	"api/controllers/memberships"
	MembershipsPlans "api/controllers/memberships/plans"
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
	membershipsCtrl := memberships.NewController(queries)
	coursesCtrl := courses.NewController(queries)
	membershipsPlansCtrl := MembershipsPlans.NewController(queries)

	hubSpotService := services.GetHubSpotService()
	customersCtrl := customers.NewController(hubSpotService)

	router := chi.NewRouter()

	router.Use(middleware.Logger)
	router.Use(middlewares.SetJSONContentType)

	registerFacilitiesRoutes(router, facilitiesCtrl)
	registerFacilitiesTypesRoutes(router, facilitiesTypesCtrl)
	registerSchedulesRoutes(router, schedulesCtrl)
	registerMembershipsRoutes(router, membershipsCtrl)
	registerCoursesRoutes(router, coursesCtrl)
	registerMembershipPlanRoutes(router, membershipsPlansCtrl)
	registerCustomersRoutes(router, customersCtrl)

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

func registerMembershipsRoutes(router *chi.Mux, controller *memberships.MembershipsController) {

	router.Route("/api/memberships", func(r chi.Router) {
		r.Get("/", controller.GetAllMemberships)
		r.Get("/{id}", controller.GetMembershipById)
		r.Post("/", controller.CreateMembership)
		r.Put("/{id}", controller.UpdateMembership)
		r.Delete("/{id}", controller.DeleteMembership)
	})
}

func registerMembershipPlanRoutes(router *chi.Mux, controller *MembershipsPlans.MembershipPlansController) {
	router.Route("/api/memberships/plans", func(r chi.Router) {
		r.Get("/", controller.GetMembershipPlans)
		r.Post("/", controller.CreateMembershipPlan)
	})
}

func registerCoursesRoutes(router *chi.Mux, controller *courses.CoursesController) {

	router.Route("/api/courses", func(r chi.Router) {
		r.Get("/", controller.GetAllCourses)
		r.Get("/{id}", controller.GetCourseById)
		r.Post("/", controller.CreateCourse)
		r.Put("/{id}", controller.UpdateCourse)
		r.Delete("/{id}", controller.DeleteCourse)
	})
}

func registerAuthRoutes(router *chi.Mux) {

	router.Route("/oauth/callback/google", func(r chi.Router) {
		r.Get("/", callback.HandleGoogleOAuthCallback)
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
