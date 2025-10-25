using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class StopRepository : BaseRepository<Stop>, IStopRepository
    {
        public StopRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Stop>> GetStopsByRouteIdAsync(Guid routeId)
        {
            return await _context.Stops
                .Where(s => s.RouteId == routeId)
                .ToListAsync();
        }
    }
}
