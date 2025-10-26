using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq; // Needed for .Any()

namespace Application.UnitTests
{
    public class BusReservationDbContextFactory
    {
        public static AppDbContext Create()
        {
            // 1. Set up options for an in-memory database.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // 2. Create the DbContext instance.
            var context = new AppDbContext(options);

            // 3. Ensure the database schema is created based on the DbContext model.
            context.Database.EnsureCreated();

            // 4. Seed data explicitly for tests.
            SeedData(context);

            return context;
        }

        private static void SeedData(AppDbContext context)
        {
            // IDs for Route, Bus, Schedule
            var routeId = Guid.Parse("ee2add3e-f635-4340-bb3c-9148d8a7c2e3");
            var busId = Guid.Parse("a548238b-d20f-488f-a0e2-763d3f94628f");
            var scheduleId = Guid.Parse("b11c34a1-0e31-4ff5-9464-3e91501b8495");

            // Seed Route (if not already present)
            if (!context.Routes.Any(r => r.Id == routeId))
            {
                context.Routes.Add(new Route("Dhaka", "Rajshahi") { Id = routeId });
            }

            // Seed Bus (if not already present)
            if (!context.Buses.Any(b => b.Id == busId))
            {
                context.Buses.Add(new Bus("National Travels", "NON AC - 101", 4, "4 hours before departure") { Id = busId }); // Consistent 4 seats
            }

            // Seed BusSchedule (if not already present - with ALL constructor arguments)
            if (!context.BusSchedules.Any(bs => bs.Id == scheduleId))
            {
                context.BusSchedules.Add(new BusSchedule(
                    busId,
                    routeId,
                    new DateTime(2025, 10, 23, 6, 0, 0, DateTimeKind.Utc), // DepartureTime
                    new DateTime(2025, 10, 23, 13, 30, 0, DateTimeKind.Utc), // ArrivalTime
                    700m,  // Price
                    "Kallyanpur", // DepartureLocation
                    "Chapai Nawabganj", // ArrivalLocation
                    20m,   // ServiceCharge
                    28m,   // PGWCharge
                    48m    // Discount
                )
                { Id = scheduleId });
            }

            // Seed Seats (if not already present for this schedule)
            if (!context.Seats.Any(s => s.BusScheduleId == scheduleId))
            {
                context.Seats.AddRange(
                    new Seat(scheduleId, "A1") { Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1") },
                    new Seat(scheduleId, "A2") { Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2") },
                    new Seat(scheduleId, "B1") { Id = Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1") },
                    new Seat(scheduleId, "B2") { Id = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2") }
                );
            }

            // Seed Stops (if not already present for this route)
            if (!context.Stops.Any(st => st.RouteId == routeId))
            {
                context.Stops.AddRange(
                    new Stop(routeId, "[06:00 AM] Kallyanpur counter", StopType.Boarding) { Id = Guid.Parse("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1") },
                    new Stop(routeId, "[06:15 AM] Mohakhali counter", StopType.Boarding) { Id = Guid.Parse("c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2") },
                    new Stop(routeId, "[10:30 AM] Baneshore Counter", StopType.Dropping) { Id = Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1") },
                    new Stop(routeId, "[12:30 PM] Rajshahi Counter", StopType.Dropping) { Id = Guid.Parse("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2") },
                    new Stop(routeId, "[01:00 PM] Rajabari Counter", StopType.Dropping) { Id = Guid.Parse("d3d3d3d3-d3d3-d3d3-d3d3-d3d3d3d3d3d3") }
                );
            }

            // Save the seeded data to the in-memory database
            context.SaveChanges();
        }

        // Method to destroy the context after a test
        public static void Destroy(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}