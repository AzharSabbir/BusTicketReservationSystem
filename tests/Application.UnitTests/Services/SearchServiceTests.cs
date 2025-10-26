using Application; 
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Shouldly; 
using System;
using System.Threading.Tasks;
using Xunit; 

namespace Application.UnitTests.Services
{
    public class SearchServiceTests : IDisposable 
    {
        private readonly AppDbContext _context;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            _context = BusReservationDbContextFactory.Create();
            _busScheduleRepository = new BusScheduleRepository(_context);
            _searchService = new SearchService(_busScheduleRepository);
        }

        [Fact] 
        public async Task SearchAvailableBusesAsync_ShouldReturn_Buses_ForValidRouteAndDate()
        {
            var from = "Dhaka";
            var to = "Rajshahi";
            var date = new DateTime(2025, 10, 23); 

            var results = await _searchService.SearchAvailableBusesAsync(from, to, date);

            results.ShouldNotBeNull();
            results.Count.ShouldBe(1);
            results[0].CompanyName.ShouldBe("National Travels");
            results[0].SeatsLeft.ShouldBe(4);
        }

        [Fact]
        public async Task SearchAvailableBusesAsync_ShouldReturn_EmptyList_ForInvalidRoute()
        {
            var from = "Dhaka";
            var to = "Khulna"; 
            var date = new DateTime(2025, 10, 23);

            var results = await _searchService.SearchAvailableBusesAsync(from, to, date);

            results.ShouldNotBeNull();
            results.ShouldBeEmpty(); 
        }

        public void Dispose()
        {
            BusReservationDbContextFactory.Destroy(_context);
            GC.SuppressFinalize(this);
        }
    }
}