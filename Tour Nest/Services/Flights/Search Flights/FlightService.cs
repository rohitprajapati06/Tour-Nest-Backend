

using System.Text.Json;
using TourNest.Models.Flights.Search_Flights;

namespace TourNest.Services.Flights.Search_Flights
{
    public class FlightService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FlightService(HttpClient httpClient , IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(_configuration["BookingCom:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _configuration["BookingCom:Host"]);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _configuration["BookingCom:Key"]);

        }

        public async Task<FlightResponse> GetFlightAsync(
    string fromId,
    string toId,
    DateOnly departDate,
    DateOnly? returnDate,
    int? pageNo,
    int? adults,
    string? children,
    Sort sort,
    CabinClass cabinClass,
    string currency_code)
        {
            // Build the query string
            var queryParameters = new Dictionary<string, string>
    {
        { "fromId", fromId },
        { "toId", toId },
        { "departDate", departDate.ToString("yyyy-MM-dd") },
        { "returnDate", returnDate?.ToString("yyyy-MM-dd") }, // Nullable handling
        { "pageNo", pageNo?.ToString() },
        { "adults", adults?.ToString() },
        { "children", children },
        { "sort", sort.ToString().ToUpper() }, // Convert enum to string
        { "cabinClass", cabinClass.ToString().ToUpper() }, // Convert enum to string
        { "currency_code", currency_code }
    };

            // Remove null or empty query parameters
            var filteredParameters = queryParameters
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}");

            var url = _httpClient.BaseAddress + $"searchFlights?{string.Join("&", filteredParameters)}";

            // Make the HTTP GET request
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching flights: {response.StatusCode} - {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response into FlightResponse
            var flightResponse = JsonSerializer.Deserialize<FlightResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return flightResponse ?? throw new Exception("Failed to deserialize flight response");
        }

    }
}
