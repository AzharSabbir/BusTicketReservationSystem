using Application; // Namespace for SearchService
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Shouldly; // Assertion library
using System;
using System.Threading.Tasks;
using Xunit; // Test framework

namespace Application.UnitTests.Services
{
    public class SearchServiceTests : IDisposable // Implement IDisposable for cleanup
    {
        private readonly AppDbContext _context;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            // Arrange (Global): Create context and real repository using the factory
            _context = BusReservationDbContextFactory.Create();
            _busScheduleRepository = new BusScheduleRepository(_context);
            _searchService = new SearchService(_busScheduleRepository);
        }

        [Fact] // Marks this as a test method
        public async Task SearchAvailableBusesAsync_ShouldReturn_Buses_ForValidRouteAndDate()
        {
            // Arrange (Local): Define search criteria matching seed data
            var from = "Dhaka";
            var to = "Rajshahi";
            var date = new DateTime(2025, 10, 23); // Date seeded in the factory

            // Act: Call the service method
            var results = await _searchService.SearchAvailableBusesAsync(from, to, date);

            // Assert: Check the results using Shouldly
            results.ShouldNotBeNull();
            results.Count.ShouldBe(1); // Expecting one bus schedule from seed data
            results[0].CompanyName.ShouldBe("National Travels");
            results[0].SeatsLeft.ShouldBe(4); // Seeded Bus has 4 seats total, 0 booked
        }

        [Fact]
        public async Task SearchAvailableBusesAsync_ShouldReturn_EmptyList_ForInvalidRoute()
        {
            // Arrange
            var from = "Dhaka";
            var to = "Khulna"; // Route not seeded
            var date = new DateTime(2025, 10, 23);

            // Act
            var results = await _searchService.SearchAvailableBusesAsync(from, to, date);

            // Assert
            results.ShouldNotBeNull();
            results.ShouldBeEmpty(); // Expecting an empty list
        }

        // Cleanup method called after each test
        public void Dispose()
        {
            BusReservationDbContextFactory.Destroy(_context);
            GC.SuppressFinalize(this);
        }
    }
}