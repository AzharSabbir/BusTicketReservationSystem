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
    public class BusScheduleRepository : IBusScheduleRepository
    {
        private readonly AppDbContext _context;
        public BusScheduleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BusSchedule>> GetSchedulesByRouteAsync(string from, string to, DateTime journeyDate)
        {
            var schedules = await _context.BusSchedules

                .Include(s => s.Bus)
                .Include(s => s.Seats)
                .Include(s => s.Route)
                .Where(s => s.Route.From == from &&
                            s.Route.To == to &&
                            s.DepartureTime.Date == journeyDate.Date)

                .ToListAsync();

            return schedules;
        }
    }
}
