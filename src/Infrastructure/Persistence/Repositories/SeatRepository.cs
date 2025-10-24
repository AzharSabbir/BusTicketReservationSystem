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
    public class SeatRepository : BaseRepository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Seat>> GetSeatsByScheduleIdAsync(Guid scheduleId)
        {
            return await _context.Seats
                .Where(s => s.BusScheduleId == scheduleId)
                .ToListAsync();
        }
    }
}
