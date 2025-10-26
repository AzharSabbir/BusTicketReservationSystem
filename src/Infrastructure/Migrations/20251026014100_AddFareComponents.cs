using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFareComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"));

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

            migrationBuilder.AddColumn<string>(
                name: "ArrivalLocation",
                table: "BusSchedules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartureLocation",
                table: "BusSchedules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancellationPolicy",
                table: "Buses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropColumn(
                name: "ArrivalLocation",
                table: "BusSchedules");

            migrationBuilder.DropColumn(
                name: "DepartureLocation",
                table: "BusSchedules");

            migrationBuilder.DropColumn(
                name: "CancellationPolicy",
                table: "Buses");

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

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "BusScheduleId", "SeatNumber", "Status" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"), "A1", 0 },
                    { new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"), "A2", 0 },
                    { new Guid("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"), new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"), "B1", 0 },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"), "B2", 0 }
                });
        }
    }
}
