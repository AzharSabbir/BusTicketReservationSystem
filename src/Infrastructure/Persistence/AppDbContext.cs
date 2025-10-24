using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<BusSchedule> BusSchedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var routeId = Guid.Parse("ee2add3e-f635-4340-bb3c-9148d8a7c2e3");
            var busId = Guid.Parse("a548238b-d20f-488f-a0e2-763d3f94628f");
            var scheduleId = Guid.Parse("b11c34a1-0e31-4ff5-9464-3e91501b8495");

            modelBuilder.Entity<Route>().HasData(
                new Route("Dhaka", "Rajshahi") { Id = routeId }
            );

            modelBuilder.Entity<Bus>().HasData(
                new Bus("National Travels", "NON AC - 101", 40) { Id = busId }
            );

            modelBuilder.Entity<BusSchedule>().HasData(
                new BusSchedule(busId, routeId,
                    new DateTime(2025, 10, 23, 6, 0, 0, DateTimeKind.Utc),
                    new DateTime(2025, 10, 23, 13, 30, 0, DateTimeKind.Utc),
                    700m)
                {
                    Id = scheduleId
                }
            );

            modelBuilder.Entity<Seat>().HasData(
                new Seat(scheduleId, "A1") { Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1") },
                new Seat(scheduleId, "A2") { Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2") },
                new Seat(scheduleId, "B1") { Id = Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1") },
                new Seat(scheduleId, "B2") { Id = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2") }
            );
        }
    }
}
