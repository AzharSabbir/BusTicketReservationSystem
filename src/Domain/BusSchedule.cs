using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BusSchedule : BaseEntity
    {
        public Guid BusId { get; private set; }
        public Bus Bus { get; private set; }

        public Guid RouteId { get; private set; }
        public Route Route { get; private set; }

        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public decimal Price { get; private set; }

        public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
        private readonly List<Seat> _seats = new List<Seat>();

        private BusSchedule() { }
        public BusSchedule(Guid busId, Guid routeId, DateTime departureTime, DateTime arrivalTime, decimal price)
        {
            BusId = busId;
            RouteId = routeId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Price = price;
        }

        public void CreateSeats(int totalSeats)
        {
            _seats.Clear();
            for (int i = 1; i <= totalSeats; i++)
            {
                _seats.Add(new Seat(this.Id, $"S{i}")); 
            }
        }
    }
}
