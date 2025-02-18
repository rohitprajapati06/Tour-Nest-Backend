namespace TourNest.Services.Location;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TourNest.Models.Location;

public class InstagramScraperService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<InstagramScraperService> _logger;

    public InstagramScraperService(HttpClient httpClient, IConfiguration configuration, ILogger<InstagramScraperService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Location:Key"];
        _logger = logger;
    }

    public async Task<LocationResponse> SearchLocationAsync(string query)
    {
        var url = $"https://instagram-scraper-api2.p.rapidapi.com/v1/search_location?search_query={query}";

        _logger.LogInformation("Requesting URL: {Url}", url);  // Log the request URL

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("x-rapidapi-host", "instagram-scraper-api2.p.rapidapi.com");
        request.Headers.Add("x-rapidapi-key", _apiKey);

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LocationResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
