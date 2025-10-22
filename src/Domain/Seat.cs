using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Seat : BaseEntity
    {
        public Guid BusScheduleId { get; private set; }
        public string SeatNumber { get; private set; }
        public SeatStatus Status { get; private set; }

        private Seat() { }
        public Seat(Guid busScheduleId, string seatNumber)
        {
            BusScheduleId = busScheduleId;
            SeatNumber = seatNumber;
            Status = SeatStatus.Available; 
        }
        public bool BookSeat()
        {
            if (Status == SeatStatus.Available)
            {
                Status = SeatStatus.Booked;
                return true; 
            }
            return false; 
        }
    }
}
