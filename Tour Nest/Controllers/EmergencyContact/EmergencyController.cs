using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourNest.Models;

namespace TourNest.Controllers.EmergencyContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyController : ControllerBase
    {
        private readonly TourNestContext context;

        public EmergencyController(TourNestContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await context.EmergencyContacts.ToListAsync();
            return Ok(data);
        }
    }
}
