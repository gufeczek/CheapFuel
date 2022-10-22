using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class CreateTableFor_ServiceAtStation_OwnedStation_Favorite_FuelAtStation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StationChainId",
                table: "FuelStations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => new { x.UserId, x.FuelStationId });
                    table.ForeignKey(
                        name: "FK_Favorite_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorite_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FuelAtStation",
                columns: table => new
                {
                    FuelTypeId = table.Column<long>(type: "bigint", nullable: false),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelAtStation", x => new { x.FuelTypeId, x.FuelStationId });
                    table.ForeignKey(
                        name: "FK_FuelAtStation_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelAtStation_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OwnedStation",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedStation", x => new { x.UserId, x.FuelStationId });
                    table.ForeignKey(
                        name: "FK_OwnedStation_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnedStation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceAtStation",
                columns: table => new
                {
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAtStation", x => new { x.ServiceId, x.FuelStationId });
                    table.ForeignKey(
                        name: "FK_ServiceAtStation_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceAtStation_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_StationChainId",
                table: "FuelStations",
                column: "StationChainId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_FuelStationId",
                table: "Favorite",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelAtStation_FuelStationId",
                table: "FuelAtStation",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedStation_FuelStationId",
                table: "OwnedStation",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAtStation_FuelStationId",
                table: "ServiceAtStation",
                column: "FuelStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelStations_StationChains_StationChainId",
                table: "FuelStations",
                column: "StationChainId",
                principalTable: "StationChains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelStations_StationChains_StationChainId",
                table: "FuelStations");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "FuelAtStation");

            migrationBuilder.DropTable(
                name: "OwnedStation");

            migrationBuilder.DropTable(
                name: "ServiceAtStation");

            migrationBuilder.DropIndex(
                name: "IX_FuelStations_StationChainId",
                table: "FuelStations");

            migrationBuilder.DropColumn(
                name: "StationChainId",
                table: "FuelStations");
        }
    }
}
