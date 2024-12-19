package customers

import (
	"api/services"
	"encoding/json"
	"net/http"
)

type CustomerController struct {
	hubSpotService *services.HubSpotService
}

func NewController(service *services.HubSpotService) *CustomerController {
	return &CustomerController{hubSpotService: service}
}

func (c *CustomerController) GetCustomers(w http.ResponseWriter, _ *http.Request) {
	// Fetch customers using HubSpotService
	customers, err := c.hubSpotService.GetCustomers("")
	if err != nil {
		http.Error(w, "Failed to retrieve customers: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(customers); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}
