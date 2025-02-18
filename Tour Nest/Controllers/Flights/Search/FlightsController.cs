using Microsoft.AspNetCore.Mvc;
using TourNest.Models.Flights.Flight_Search_Request;
using TourNest.Services.Flights.Search_Flights;

namespace TourNest.Controllers.Flights.Search;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly FlightService _flightService;

    public FlightsController(FlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights(
            [FromQuery] DateOnly departDate,
            [FromQuery] DateOnly? returnDate,
            [FromQuery] string fromId = "BOM.AIRPORT",
            [FromQuery] string toId = "DEL.AIRPORT",
            [FromQuery] int? pageNo=1,
            [FromQuery] int? adults=1,
            [FromQuery] string? children = "0",
            [FromQuery] Sort sort = Sort.BEST, // Default to BEST sort
            [FromQuery] CabinClass cabinClass = CabinClass.ECONOMY, // Default to Economy
            [FromQuery] string currency_code = "INR" 
        )
    {
        try
        {
            // Call the service to fetch flight data
            var flightResponse = await _flightService.GetFlightAsync(
                fromId,
                toId,
                departDate,
                returnDate,
                pageNo,
                adults,
                children,
                sort,
                cabinClass,
                currency_code
            );

            // Return the response to the client
            return Ok(flightResponse);
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            // Return an error response
            return StatusCode(500, new { Message = "An error occurred while fetching flights", Details = ex.Message });
        }
    }

}
