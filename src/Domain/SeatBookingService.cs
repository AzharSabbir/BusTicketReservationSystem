using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class SeatBookingService
    {
        public Ticket PerformBooking(Seat seat, Passenger passenger, string boardingPoint, string droppingPoint)
        {
            if (seat.Status != SeatStatus.Available)
            {
                throw new InvalidOperationException("This seat is not available for booking.");
            }

            seat.BookSeat(); 

            var newTicket = new Ticket(
                passenger.Id,
                seat.Id,
                seat.BusScheduleId,
                boardingPoint,
                droppingPoint
            );

            return newTicket;
        }
    }
}
