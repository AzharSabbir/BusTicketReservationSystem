using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Ticket : BaseEntity
    {
        public Guid PassengerId { get; private set; }
        public Passenger Passenger { get; private set; }

        public Guid SeatId { get; private set; }
        public Seat Seat { get; private set; }

        public Guid BusScheduleId { get; private set; }
        public BusSchedule BusSchedule { get; private set; }

        public string BoardingPoint { get; private set; }
        public string DroppingPoint { get; private set; }

        private Ticket() { }

        public Ticket(Guid passengerId, Guid seatId, Guid busScheduleId, string boardingPoint, string droppingPoint)
        {
            PassengerId = passengerId;
            SeatId = seatId;
            BusScheduleId = busScheduleId;
            BoardingPoint = boardingPoint;
            DroppingPoint = droppingPoint;
        }
    }
}
