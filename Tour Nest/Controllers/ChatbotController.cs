using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TourNest.Models.Chatbot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TourNest.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly RapidApiSettings _rapidApiSettings;

    public ChatController(HttpClient httpClient, IOptions<RapidApiSettings> rapidApiOptions)
    {
        _httpClient = httpClient;
        _rapidApiSettings = rapidApiOptions.Value;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] UserMessageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User session not found.");
        }

        var apiUrl = "https://chatgpt-42.p.rapidapi.com/gpt4";
        var requestBody = new
        {
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = request.Content
                }
            },
            web_access = false
        };

        var jsonBody = JsonSerializer.Serialize(requestBody);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
        };

        // Add headers from the configuration
        requestMessage.Headers.Add("x-rapidapi-host", _rapidApiSettings.Host);
        requestMessage.Headers.Add("x-rapidapi-key", _rapidApiSettings.Key);
        requestMessage.Headers.Add("x-rapidapi-ua", _rapidApiSettings.UA);

        using var response = await _httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, errorContent);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return Ok(new { UserId = userId, Response = JsonDocument.Parse(responseContent) });
    }
}

