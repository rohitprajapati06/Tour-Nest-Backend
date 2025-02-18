using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourNest.Models.Flights.Search_Flights;
using TourNest.Services.Flights.Book_Flights;

namespace TourNest.Controllers.Flights.Book_Flights;

[ApiController]
[Route("api/[controller]")]
public class FlightBookingController : ControllerBase
{
    private readonly FlightBookingService _flightBookingService;

    // Inject the FlightBookingService via constructor
    public FlightBookingController(FlightBookingService flightBookingService )
    {
        _flightBookingService = flightBookingService;
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookFlight([FromBody] FlightOffers flightOffer, [FromQuery] string email, [FromQuery] string phone)
    {
        try
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
                return BadRequest("Email and phone are required.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
                return Unauthorized(new { message = "Invalid or missing User ID." });

            var bookingRequest = await _flightBookingService.MapFlightOfferToBookingRequestAsync(flightOffer, email, phone, parsedUserId);

            return Ok(new { message = "Booking successful!", booking = bookingRequest });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

}