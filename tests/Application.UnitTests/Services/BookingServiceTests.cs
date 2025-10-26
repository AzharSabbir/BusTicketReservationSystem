using Application; // Namespace for BookingService
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
        // Dependencies
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeatRepository _seatRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly IStopRepository _stopRepository;
        private readonly SeatBookingService _seatBookingService;
        private readonly IBookingService _bookingService; // Service under test

        // Seeded IDs
        private readonly Guid _scheduleId = Guid.Parse("b11c34a1-0e31-4ff5-9464-3e91501b8495");
        private readonly Guid _seatA1Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
        private readonly Guid _seatA2Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2");

        public BookingServiceTests()
        {
            // Arrange (Global): Create context and real dependencies
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
            // Arrange
            var input = new BookSeatInputDto
            {
                BusScheduleId = _scheduleId,
                SeatId = _seatA1Id, // A1 is available
                PassengerName = "Test User One",
                PassengerMobile = "01111111111",
                BoardingPoint = "[06:00 AM] Kallyanpur counter",
                DroppingPoint = "[12:30 PM] Rajshahi Counter"
            };

            // Act
            var result = await _bookingService.BookSeatAsync(input);

            // Assert
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
            // Arrange: Book seat A2 first
            var firstInput = new BookSeatInputDto { BusScheduleId = _scheduleId, SeatId = _seatA2Id, /* other details */ PassengerName = "User A", PassengerMobile = "1", BoardingPoint = "B1", DroppingPoint = "D1" };
            await _bookingService.BookSeatAsync(firstInput);

            // Arrange: Try to book A2 again
            var secondInput = new BookSeatInputDto
            {
                BusScheduleId = _scheduleId,
                SeatId = _seatA2Id, // A2 is now booked
                PassengerName = "Test User Two",
                PassengerMobile = "02222222222",
                BoardingPoint = "[06:15 AM] Mohakhali counter",
                DroppingPoint = "[01:00 PM] Rajabari Counter"
            };

            // Act
            var result = await _bookingService.BookSeatAsync(secondInput);

            // Assert
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("This seat is not available for booking.");
            var seatInDb = await _context.Seats.FindAsync(_seatA2Id);
            seatInDb?.Status.ShouldBe(SeatStatus.Booked); // Still booked by first user
            _context.Tickets.Count().ShouldBe(1); // Only one ticket should exist
        }

        public void Dispose()
        {
            BusReservationDbContextFactory.Destroy(_context);
            GC.SuppressFinalize(this);
        }
    }
}