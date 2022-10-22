using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class CreateTableFor_FuelPricec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Price = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Available = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: true),
                    FuelTypeId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelPrices_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuelPrices_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuelPrices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_FuelStationId",
                table: "FuelPrices",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_FuelTypeId",
                table: "FuelPrices",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_UserId",
                table: "FuelPrices",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelPrices");
        }
    }
}
