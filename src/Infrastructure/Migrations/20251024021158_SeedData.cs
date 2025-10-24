using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "Id", "BusName", "CompanyName", "TotalSeats" },
                values: new object[] { new Guid("a548238b-d20f-488f-a0e2-763d3f94628f"), "NON AC - 101", "National Travels", 40 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "Id", "From", "To" },
                values: new object[] { new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), "Dhaka", "Rajshahi" });

            migrationBuilder.InsertData(
                table: "BusSchedules",
                columns: new[] { "Id", "ArrivalTime", "BusId", "DepartureTime", "Price", "RouteId" },
                values: new object[] { new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"), new DateTime(2025, 10, 23, 13, 30, 0, 0, DateTimeKind.Utc), new Guid("a548238b-d20f-488f-a0e2-763d3f94628f"), new DateTime(2025, 10, 23, 6, 0, 0, 0, DateTimeKind.Utc), 700m, new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusSchedules",
                keyColumn: "Id",
                keyValue: new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"));

            migrationBuilder.DeleteData(
                table: "Buses",
                keyColumn: "Id",
                keyValue: new Guid("a548238b-d20f-488f-a0e2-763d3f94628f"));

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumn: "Id",
                keyValue: new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"));
        }
    }
}
