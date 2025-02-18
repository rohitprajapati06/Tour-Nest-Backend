using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using TourNest.Models.Flights.Search_Flights;

namespace TourNest.Services.Flights.Search_Flights_Locations;

// Services/SearchFlightService.cs
public class SearchFlightService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SearchFlightService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        // Set base URL and headers
        _httpClient.BaseAddress = new Uri(_configuration["BookingCom:BaseUrl"]);
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _configuration["BookingCom:Host"]);
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _configuration["BookingCom:Key"]);

    }

    public async Task<ResponseWrapper> SearchAsync(string query)
    {
        // Corrected URL
        var url = $"searchDestination?query={Uri.EscapeDataString(query)}";

        // Log the full URL for debugging
        Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{url}");

        // Make the API request
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch data: {response.StatusCode}, Content: {errorContent}");
        }

        // Deserialize the response
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResponseWrapper>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }


}
