using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly AppDbContext _context;

        public SearchController(ISearchService searchService, AppDbContext context)
        {
            _searchService = searchService;
            _context = context;
        }

        [HttpGet("available-buses")] 
        public async Task<ActionResult<List<AvailableBusDto>>> Search(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] DateTime journeyDate)
        {
            var results = await _searchService.SearchAvailableBusesAsync(from, to, journeyDate);

            return Ok(results); 
        }

        [HttpGet("locations")]
        public async Task<ActionResult<RouteLocationsDto>> GetLocations()
        {
            var fromLocations = await _context.Routes
                                        .Select(r => r.From)
                                        .Distinct()
                                        .OrderBy(loc => loc) 
                                        .ToListAsync();

            var toLocations = await _context.Routes
                                      .Select(r => r.To)
                                      .Distinct()
                                      .OrderBy(loc => loc)
                                      .ToListAsync();

            var result = new RouteLocationsDto
            {
                FromLocations = fromLocations,
                ToLocations = toLocations
            };

            return Ok(result);
        }
    }
}
