using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class RenameTable_OpeningClosingTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningClosingTime_FuelStations_FuelStationId",
                table: "OpeningClosingTime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningClosingTime",
                table: "OpeningClosingTime");

            migrationBuilder.RenameTable(
                name: "OpeningClosingTime",
                newName: "OpeningClosingTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningClosingTimes",
                table: "OpeningClosingTimes",
                columns: new[] { "FuelStationId", "DayOfWeek" });

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningClosingTimes_FuelStations_FuelStationId",
                table: "OpeningClosingTimes",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningClosingTimes_FuelStations_FuelStationId",
                table: "OpeningClosingTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpeningClosingTimes",
                table: "OpeningClosingTimes");

            migrationBuilder.RenameTable(
                name: "OpeningClosingTimes",
                newName: "OpeningClosingTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpeningClosingTime",
                table: "OpeningClosingTime",
                columns: new[] { "FuelStationId", "DayOfWeek" });

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningClosingTime_FuelStations_FuelStationId",
                table: "OpeningClosingTime",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
