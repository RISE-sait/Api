package callback

import (
	"encoding/json"
	"net/http"
	"net/url"
	"strings"
)

// Structure to store the access token response
type OAuthTokenResponse struct {
	AccessToken  string `json:"access_token"`
	TokenType    string `json:"token_type"`
	RefreshToken string `json:"refresh_token,omitempty"`
	ExpiresIn    int    `json:"expires_in"`
}

type UserInfo struct {
	ID    string `json:"id"`
	Name  string `json:"name"`
	Email string `json:"email"`
}

func HandleOAuthCallback(w http.ResponseWriter, r *http.Request) {
	// Retrieve the authorization code from the query parameters
	code := r.URL.Query().Get("code")
	if code == "" {
		http.Error(w, "Missing authorization code", http.StatusBadRequest)
		return
	}

	// Prepare the request to exchange the authorization code for an access token
	tokenURL := "https://oauth.provider.com/token" // Change to your OAuth provider's token endpoint
	clientID := "your-client-id"
	clientSecret := "your-client-secret"

	data := url.Values{
		"grant_type":    {"authorization_code"},
		"code":          {code},
		"client_id":     {clientID},
		"client_secret": {clientSecret},
	}

	// Make the request to exchange the authorization code for an access token
	resp, err := http.Post(tokenURL, "application/x-www-form-urlencoded", strings.NewReader(data.Encode()))
	if err != nil {
		http.Error(w, "Failed to exchange authorization code for token", http.StatusInternalServerError)
		return
	}
	defer resp.Body.Close()

	// Decode the access token response
	var tokenResponse OAuthTokenResponse
	if err := json.NewDecoder(resp.Body).Decode(&tokenResponse); err != nil {
		http.Error(w, "Failed to decode token response", http.StatusInternalServerError)
		return
	}

	// Use the access token to get user info (example for Google)
	userInfoURL := "https://www.googleapis.com/oauth2/v2/userinfo"
	req, err := http.NewRequest("GET", userInfoURL, nil)
	if err != nil {
		http.Error(w, "Failed to create user info request", http.StatusInternalServerError)
		return
	}
	req.Header.Set("Authorization", "Bearer "+tokenResponse.AccessToken)

	client := &http.Client{}
	userInfoResp, err := client.Do(req)
	if err != nil {
		http.Error(w, "Failed to get user info", http.StatusInternalServerError)
		return
	}
	defer userInfoResp.Body.Close()

	// Decode the user info response
	var userInfo UserInfo
	if err := json.NewDecoder(userInfoResp.Body).Decode(&userInfo); err != nil {
		http.Error(w, "Failed to decode user info response", http.StatusInternalServerError)
		return
	}

	// Respond with the user info
	w.Header().Set("Content-Type", "application/json")
	if err := json.NewEncoder(w).Encode(userInfo); err != nil {
		http.Error(w, "Failed to encode user info response", http.StatusInternalServerError)
	}
}
