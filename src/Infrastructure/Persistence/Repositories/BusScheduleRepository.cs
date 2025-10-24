using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class BusScheduleRepository : BaseRepository<BusSchedule>, IBusScheduleRepository
    {
        public BusScheduleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<BusSchedule>> GetSchedulesByRouteAsync(string from, string to, DateTime journeyDate)
        {
            var searchDate = DateTime.SpecifyKind(journeyDate.Date, DateTimeKind.Utc);

            var schedules = await _context.BusSchedules
                .Include(s => s.Bus)
                .Include(s => s.Seats)
                .Include(s => s.Route)
                .Where(s => s.Route.From == from &&
                            s.Route.To == to &&
                            s.DepartureTime.Date == searchDate)
                .ToListAsync();

            return schedules;
        }

        public async Task<BusSchedule?> GetScheduleWithBusAndSeatsAsync(Guid scheduleId)
        {
            return await _context.BusSchedules
                .Include(s => s.Bus)    
                .Include(s => s.Seats)  
                .FirstOrDefaultAsync(s => s.Id == scheduleId);
        }
    }
}