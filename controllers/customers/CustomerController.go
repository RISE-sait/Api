package customers

import (
	"api/services"
	"encoding/json"
	"fmt"
	"net/http"
	"time"
)

type CustomerController struct {
	hubSpotService *services.HubSpotService
}

type HubSpotCustomer struct {
	ID         string                 `json:"id"`
	Properties HubSpotCustomerDetails `json:"properties"`
}

type HubSpotCustomerDetails struct {
	UpdatedAt time.Time `json:"updatedAt"`
	FirstName string    `json:"firstName"`
	LastName  string    `json:"lastName"`
	Email     string    `json:"email"`
}

type HubSpotCustomerResponse struct {
	Results []HubSpotCustomer `json:"results"`
}

func NewController(service *services.HubSpotService) *CustomerController {
	return &CustomerController{hubSpotService: service}
}

func (c *CustomerController) GetCustomers(w http.ResponseWriter, _ *http.Request) {
	// Fetch customers using HubSpotService
	customers, err := getCustomers(services.GetHubSpotService(), "")
	if err != nil {
		http.Error(w, "Failed to retrieve customers: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(customers); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

func getCustomers(s *services.HubSpotService, after string) ([]HubSpotCustomer, error) {
	url := fmt.Sprintf("%scrm/v3/objects/contacts?limit=10", s.BaseURL)
	if after != "" {
		url += fmt.Sprintf("&after=%s", after)
	}

	req, err := http.NewRequest(http.MethodGet, url, nil)
	if err != nil {
		return nil, err
	}
	req.Header.Set("Authorization", "Bearer "+s.ApiKey)
	req.Header.Set("Accept", "application/json")

	resp, err := s.Client.Do(req)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("failed to fetch customers, status code: %d", resp.StatusCode)
	}

	var hubSpotResponse HubSpotCustomerResponse
	if err := json.NewDecoder(resp.Body).Decode(&hubSpotResponse); err != nil {
		return nil, fmt.Errorf("failed to decode response: %w", err)
	}

	return hubSpotResponse.Results, nil
}
