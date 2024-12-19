package products

import (
	"api/services"
	"encoding/json"
	"fmt"
	"net/http"
)

type ProductsController struct {
	hubSpotService *services.HubSpotService
}

// HubSpotProduct represents a product in HubSpot.
type HubSpotProduct struct {
	ID         string                `json:"id"`
	Properties HubSpotProductDetails `json:"properties"`
}

// HubSpotProductDetails represents the details of a product.
type HubSpotProductDetails struct {
	Name        string `json:"name"`
	Description string `json:"description"`
	Price       string `json:"price"`
	ProductType string `json:"hs_product_type"`
}

// HubSpotProductResponse represents the response from the HubSpot API for products.
type HubSpotProductResponse struct {
	Results []HubSpotProduct `json:"results"`
}

func NewController(service *services.HubSpotService) *ProductsController {
	return &ProductsController{hubSpotService: service}
}

func (c *ProductsController) GetProducts(w http.ResponseWriter, _ *http.Request) {
	// Fetch customers using HubSpotService
	products, err := getProducts(services.GetHubSpotService())
	if err != nil {
		http.Error(w, "Failed to retrieve products: "+err.Error(), http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(products); err != nil {
		http.Error(w, "Failed to encode response: "+err.Error(), http.StatusInternalServerError)
	}
}

// GetProducts retrieves products from HubSpot.
func getProducts(s *services.HubSpotService) ([]HubSpotProduct, error) {
	fields := "name,description,price,hs_product_type"

	url := fmt.Sprintf("%scrm/v3/objects/products?properties=%s", s.BaseURL, fields)

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
		return nil, fmt.Errorf("failed to fetch products, status code: %d", resp.StatusCode)
	}

	var hubSpotResponse HubSpotProductResponse
	if err := json.NewDecoder(resp.Body).Decode(&hubSpotResponse); err != nil {
		return nil, fmt.Errorf("failed to decode response: %w", err)
	}

	return hubSpotResponse.Results, nil
}

// // CreateProduct creates a new product in HubSpot.
// func (s *HubSpotService) CreateProduct(product HubSpotProductDetails) (*HubSpotProduct, error) {
// 	url := fmt.Sprintf("%scrm/v3/objects/products?hapikey=%s", s.baseURL, s.apiKey)

// 	productData := map[string]interface{}{
// 		"properties": product,
// 	}

// 	data, err := json.Marshal(productData)
// 	if err != nil {
// 		return nil, err
// 	}

// 	resp, err := s.client.Post(url, "application/json", bytes.NewBuffer(data))
// 	if err != nil {
// 		return nil, err
// 	}
// 	defer resp.Body.Close()

// 	if resp.StatusCode != http.StatusCreated {
// 		return nil, fmt.Errorf("failed to create product: %s", resp.Status)
// 	}

// 	var createdProduct HubSpotProduct
// 	if err := json.NewDecoder(resp.Body).Decode(&createdProduct); err != nil {
// 		return nil, err
// 	}

// 	return &createdProduct, nil
// }
