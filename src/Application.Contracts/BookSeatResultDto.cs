using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public class BookSeatResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? TicketId { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
    }
}
