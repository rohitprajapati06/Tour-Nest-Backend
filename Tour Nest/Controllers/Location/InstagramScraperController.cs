using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TourNest.Services.Location;
namespace TourNest.Controllers.Location;



[Route("api/[controller]")]
[ApiController]
public class InstagramScraperController : ControllerBase
{
    private readonly InstagramScraperService _scraperService;

    public InstagramScraperController(InstagramScraperService scraperService)
    {
        _scraperService = scraperService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchLocation([FromQuery] string search_query)
    {
        var result = await _scraperService.SearchLocationAsync(search_query);
        return Ok(result);
    }
}
