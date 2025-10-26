using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public class BookSeatInputDto
    {
        public Guid BusScheduleId { get; set; }
        public Guid SeatId { get; set; }

        public string PassengerName { get; set; }
        public string PassengerMobile { get; set; }

        public string BoardingPoint { get; set; }
        public string DroppingPoint { get; set; }
    }
}
