using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain; 

namespace Application.Contracts.Persistence
{
    public interface IBusScheduleRepository
    {
        Task<List<BusSchedule>> GetSchedulesByRouteAsync(string from, string to, DateTime journeyDate);
        Task<BusSchedule?> GetScheduleWithBusAndSeatsAsync(Guid scheduleId);
    }
}
