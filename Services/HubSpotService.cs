using System.Net.Http.Headers;
using System.Text.Json;

namespace Api.Services
{
    public class HubSpotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HubSpotService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["HubSpot:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "HubSpot:ApiKey is missing in configuration");

            _httpClient.BaseAddress = new Uri("https://api.hubapi.com/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }


        public async Task<IEnumerable<HubSpotCustomer>> GetCustomers(string? after = null)
        {
            try
            {
                var url = "crm/v3/objects/contacts?limit=10";

                if (!string.IsNullOrEmpty(after))
                {
                    url += $"&after={after}";
                }

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<HubSpotResponse>(content, _jsonOptions);

                return customers?.Results ?? [];
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Failed to deserialize the response from HubSpot", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get customers from HubSpot", ex);
            }
        }
    }

    public record HubSpotResponse
    {
        public required IEnumerable<HubSpotCustomer> Results { get; set; }
    }

    public record HubSpotCustomer
    {
        public required string Id { get; set; }
        public required HubSpotCustomerProperties Properties { get; set; }
    }

    public record HubSpotCustomerProperties
    {
        public DateTime UpdatedAt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}