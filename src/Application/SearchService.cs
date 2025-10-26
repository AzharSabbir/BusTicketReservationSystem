using Domain;
using Application.Contracts; 
using Application.Contracts.Persistence; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SearchService : ISearchService 
    {
        private readonly IBusScheduleRepository _scheduleRepository;
        public SearchService(IBusScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public async Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate) // [cite: 49]
        {
            var schedules = await _scheduleRepository.GetSchedulesByRouteAsync(from, to, journeyDate);

            var results = new List<AvailableBusDto>();
            foreach (var schedule in schedules)
            {
                int seatsLeft = schedule.Bus.TotalSeats - schedule.Seats.Count(s => s.Status != SeatStatus.Available);

                results.Add(new AvailableBusDto
                {
                    BusScheduleId = schedule.Id,
                    CompanyName = schedule.Bus.CompanyName, 
                    BusName = schedule.Bus.BusName,         
                    StartTime = schedule.DepartureTime,    
                    ArrivalTime = schedule.ArrivalTime,    
                    Price = schedule.Price,                 
                    SeatsLeft = seatsLeft,
                    DepartureLocation = schedule.DepartureLocation,
                    ArrivalLocation = schedule.ArrivalLocation,
                    CancellationPolicy = schedule.Bus.CancellationPolicy
                });
            }
            return results;
        }
    }
}