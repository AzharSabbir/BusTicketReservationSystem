using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStopsAndPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "BusSchedules",
                keyColumn: "Id",
                keyValue: new Guid("b11c34a1-0e31-4ff5-9464-3e91501b8495"),
                columns: new[] { "ArrivalLocation", "DepartureLocation" },
                values: new object[] { "Chapai Nawabganj", "Kallyanpur" });

            migrationBuilder.UpdateData(
                table: "Buses",
                keyColumn: "Id",
                keyValue: new Guid("a548238b-d20f-488f-a0e2-763d3f94628f"),
                column: "CancellationPolicy",
                value: "4 hours before departure");

            migrationBuilder.InsertData(
                table: "Stops",
                columns: new[] { "Id", "Name", "RouteId", "Type" },
                values: new object[,]
                {
                    { new Guid("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"), "[06:00 AM] Kallyanpur counter", new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), 0 },
                    { new Guid("c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2"), "[06:15 AM] Mohakhali counter", new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), 0 },
                    { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "[10:30 AM] Baneshore Counter", new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), 1 },
                    { new Guid("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"), "[12:30 PM] Rajshahi Counter", new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), 1 },
                    { new Guid("d3d3d3d3-d3d3-d3d3-d3d3-d3d3d3d3d3d3"), "[01:00 PM] Rajabari Counter", new Guid("ee2add3e-f635-4340-bb3c-9148d8a7c2e3"), 1 }
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
        }
    }
}
