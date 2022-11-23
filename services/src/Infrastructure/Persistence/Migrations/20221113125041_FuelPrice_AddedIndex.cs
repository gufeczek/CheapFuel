using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class FuelPrice_AddedIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FuelPrices",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_CreatedAt",
                table: "FuelPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_Status_FuelTypeId_Available_Price",
                table: "FuelPrices",
                columns: new[] { "Status", "FuelTypeId", "Available", "Price" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelPrices_CreatedAt",
                table: "FuelPrices");

            migrationBuilder.DropIndex(
                name: "IX_FuelPrices_Status_FuelTypeId_Available_Price",
                table: "FuelPrices");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FuelPrices",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
