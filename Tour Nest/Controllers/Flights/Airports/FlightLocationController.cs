using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourNest.Services.Flights.Search_Flights_Locations;

namespace TourNest.Controllers.Flights.Airports
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightLocationController : ControllerBase
    {
        private readonly SearchFlightService searchFlightService;

        public FlightLocationController(SearchFlightService searchFlightService)
        {
            this.searchFlightService = searchFlightService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            try
            {
                var result = await searchFlightService.SearchAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
