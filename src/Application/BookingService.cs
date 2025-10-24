using Application.Contracts;
using Application.Contracts.Persistence;
using Domain;
using Domain.Services; 

namespace Application
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeatRepository _seatRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBusScheduleRepository _busScheduleRepository;

        private readonly SeatBookingService _seatBookingService;

        public BookingService(
            IUnitOfWork unitOfWork,
            ISeatRepository seatRepository,
            IPassengerRepository passengerRepository,
            ITicketRepository ticketRepository,
            IBusScheduleRepository busScheduleRepository,
            SeatBookingService seatBookingService)
        {
            _unitOfWork = unitOfWork;
            _seatRepository = seatRepository;
            _passengerRepository = passengerRepository;
            _ticketRepository = ticketRepository;
            _busScheduleRepository = busScheduleRepository;
            _seatBookingService = seatBookingService;
        }

        public async Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId)
        {
            var schedule = await _busScheduleRepository.GetScheduleWithBusAndSeatsAsync(busScheduleId);

            if (schedule == null)
            {
                return new SeatPlanDto(); 
            }

            var seatPlanDto = new SeatPlanDto
            {
                BusScheduleId = schedule.Id,
                BusName = $"{schedule.Bus.CompanyName} - {schedule.Bus.BusName}",
                Seats = schedule.Seats.Select(seat => new SeatDto
                {
                    SeatId = seat.Id,
                    SeatNumber = seat.SeatNumber,
                    Status = seat.Status.ToString() 
                }).ToList()
            };

            return seatPlanDto;
        }
        public async Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input)
        {
            try
            {
                var seat = await _seatRepository.GetByIdAsync(input.SeatId);
                if (seat == null)
                {
                    throw new InvalidOperationException("Seat not found.");
                }

                var passenger = new Passenger(input.PassengerName, input.PassengerMobile);

                await _passengerRepository.AddAsync(passenger);

                var newTicket = _seatBookingService.PerformBooking(
                    seat,
                    passenger,
                    input.BoardingPoint,
                    input.DroppingPoint
                );

                await _ticketRepository.AddAsync(newTicket);
                await _seatRepository.UpdateAsync(seat); 

                await _unitOfWork.SaveChangesAsync();

                return new BookSeatResultDto
                {
                    Success = true,
                    Message = "Booking successful!",
                    TicketId = newTicket.Id,
                    SeatNumber = seat.SeatNumber,
                    Status = seat.Status.ToString()
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BookSeatResultDto { Success = false, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new BookSeatResultDto { Success = false, Message = "An unexpected error occurred during booking." };
            }
        }
    }
}