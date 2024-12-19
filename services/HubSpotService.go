package services

import (
	"api/config"
	"encoding/json"
	"fmt"
	"net/http"
	"time"
)

// HubSpotService handles integration with HubSpot API.
type HubSpotService struct {
	client  *http.Client
	apiKey  string
	baseURL string
}

func GetHubSpotService() *HubSpotService {
	apiKey := config.Envs.HubSpotApiKey

	httpClient := &http.Client{
		Timeout: 10 * time.Second,
	}

	return &HubSpotService{
		client:  httpClient,
		apiKey:  apiKey,
		baseURL: "https://api.hubapi.com/",
	}
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

func (s *HubSpotService) GetCustomers(after string) ([]HubSpotCustomer, error) {
	url := fmt.Sprintf("%scrm/v3/objects/contacts?limit=10", s.baseURL)
	if after != "" {
		url += fmt.Sprintf("&after=%s", after)
	}

	req, err := http.NewRequest(http.MethodGet, url, nil)
	if err != nil {
		return nil, err
	}
	req.Header.Set("Authorization", "Bearer "+s.apiKey)
	req.Header.Set("Accept", "application/json")

	resp, err := s.client.Do(req)
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
