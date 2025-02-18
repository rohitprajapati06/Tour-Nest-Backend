using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourNest.Models;
using TourNest.Models.Destinations;
using TourNest.Models.TravelExpenses.PreDefined_Budgets;

namespace TourNest.Controllers.Destinations
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly TourNestContext context;

        public DestinationController(TourNestContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBudget()
        {
            var topPlaces = await context.TopPlaces.ToListAsync();
            var topDestinationsInIndia = await context.TopDestinationsInIndia.ToListAsync();
            var topTemples = await context.TopTemples.ToListAsync();

            var destination = new DestinationView
            {
                TopPlaces = topPlaces,
                TopDestinationsInIndia = topDestinationsInIndia,
                TopTemples = topTemples
            };

            return Ok(destination);
        }
    }
}
