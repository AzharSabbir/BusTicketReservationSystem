using Application; 
using Application.Contracts;
using Application.Contracts.Persistence;
using Domain;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Services
{
    public class BookingServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeatRepository _seatRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly IStopRepository _stopRepository;
        private readonly SeatBookingService _seatBookingService;
        private readonly IBookingService _bookingService; 

        private readonly Guid _scheduleId = Guid.Parse("b11c34a1-0e31-4ff5-9464-3e91501b8495");
        private readonly Guid _seatA1Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
        private readonly Guid _seatA2Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2");

        public BookingServiceTests()
        {
            _context = BusReservationDbContextFactory.Create();
            _unitOfWork = new UnitOfWork(_context);
            _seatRepository = new SeatRepository(_context);
            _passengerRepository = new PassengerRepository(_context);
            _ticketRepository = new TicketRepository(_context);
            _busScheduleRepository = new BusScheduleRepository(_context);
            _stopRepository = new StopRepository(_context);
            _seatBookingService = new SeatBookingService();
            _bookingService = new BookingService(
                _unitOfWork, _seatRepository, _passengerRepository, _ticketRepository,
                _busScheduleRepository, _stopRepository, _seatBookingService
            );
        }

        [Fact]
        public async Task BookSeatAsync_Should_Succeed_ForAvailableSeat()
        {
            var input = new BookSeatInputDto
            {
                BusScheduleId = _scheduleId,
                SeatId = _seatA1Id, 
                PassengerName = "Test User One",
                PassengerMobile = "01111111111",
                BoardingPoint = "[06:00 AM] Kallyanpur counter",
                DroppingPoint = "[12:30 PM] Rajshahi Counter"
            };

            var result = await _bookingService.BookSeatAsync(input);

            result.Success.ShouldBeTrue();
            result.Status.ShouldBe(SeatStatus.Booked.ToString());
            result.SeatNumber.ShouldBe("A1");
            var seatInDb = await _context.Seats.FindAsync(_seatA1Id);
            seatInDb?.Status.ShouldBe(SeatStatus.Booked);
            _context.Tickets.Count().ShouldBe(1);
        }

        [Fact]
        public async Task BookSeatAsync_Should_Fail_ForAlreadyBookedSeat()
        {
            var firstInput = new BookSeatInputDto { BusScheduleId = _scheduleId, SeatId = _seatA2Id, PassengerName = "User A", PassengerMobile = "1", BoardingPoint = "B1", DroppingPoint = "D1" };
            await _bookingService.BookSeatAsync(firstInput);

            var secondInput = new BookSeatInputDto
            {
                BusScheduleId = _scheduleId,
                SeatId = _seatA2Id, 
                PassengerName = "Test User Two",
                PassengerMobile = "02222222222",
                BoardingPoint = "[06:15 AM] Mohakhali counter",
                DroppingPoint = "[01:00 PM] Rajabari Counter"
            };

            var result = await _bookingService.BookSeatAsync(secondInput);

            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("This seat is not available for booking.");
            var seatInDb = await _context.Seats.FindAsync(_seatA2Id);
            seatInDb?.Status.ShouldBe(SeatStatus.Booked); 
            _context.Tickets.Count().ShouldBe(1);
        }

        public void Dispose()
        {
            BusReservationDbContextFactory.Destroy(_context);
            GC.SuppressFinalize(this);
        }
    }
}