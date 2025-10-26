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
        public decimal ServiceCharge { get; private set; }
        public decimal PGWCharge { get; private set; }
        public decimal Discount { get; private set; }

        public string DepartureLocation { get; private set; }  
        public string ArrivalLocation { get; private set; }

        public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
        private readonly List<Seat> _seats = new List<Seat>();

        private BusSchedule() { }

        // --- REPLACE THIS CONSTRUCTOR ---
        public BusSchedule(
            Guid busId,
            Guid routeId,
            DateTime departureTime,
            DateTime arrivalTime,
            decimal price, // Base price
            string departureLocation,
            string arrivalLocation,
            decimal serviceCharge, // << Add this
            decimal pgwCharge,     // << Add this
            decimal discount       // << Add this
            )
        {
            BusId = busId;
            RouteId = routeId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Price = price;
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;
            // --- ADD THESE ASSIGNMENTS ---
            ServiceCharge = serviceCharge;
            PGWCharge = pgwCharge;
            Discount = discount;
            // --- END ADDITIONS ---
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
