using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Contracts.Persistence
{
    public interface ISeatRepository : IAsyncRepository<Seat>
    {
        Task<List<Seat>> GetSeatsByScheduleIdAsync(Guid scheduleId);
    }
}
