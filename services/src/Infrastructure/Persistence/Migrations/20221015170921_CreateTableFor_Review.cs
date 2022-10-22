using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreateTableFor_Review : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_FuelStations_FuelStationId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Users_UserId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelAtStation_FuelStations_FuelStationId",
                table: "FuelAtStation");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelAtStation_FuelTypes_FuelTypeId",
                table: "FuelAtStation");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedStation_FuelStations_FuelStationId",
                table: "OwnedStation");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedStation_Users_UserId",
                table: "OwnedStation");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAtStation_FuelStations_FuelStationId",
                table: "ServiceAtStation");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAtStation_Services_ServiceId",
                table: "ServiceAtStation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAtStation",
                table: "ServiceAtStation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedStation",
                table: "OwnedStation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelAtStation",
                table: "FuelAtStation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite");

            migrationBuilder.RenameTable(
                name: "ServiceAtStation",
                newName: "ServiceAtStations");

            migrationBuilder.RenameTable(
                name: "OwnedStation",
                newName: "OwnedStations");

            migrationBuilder.RenameTable(
                name: "FuelAtStation",
                newName: "FuelAtStations");

            migrationBuilder.RenameTable(
                name: "Favorite",
                newName: "Favorites");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceAtStation_FuelStationId",
                table: "ServiceAtStations",
                newName: "IX_ServiceAtStations_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedStation_FuelStationId",
                table: "OwnedStations",
                newName: "IX_OwnedStations_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelAtStation_FuelStationId",
                table: "FuelAtStations",
                newName: "IX_FuelAtStations_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_FuelStationId",
                table: "Favorites",
                newName: "IX_Favorites_FuelStationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAtStations",
                table: "ServiceAtStations",
                columns: new[] { "ServiceId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedStations",
                table: "OwnedStations",
                columns: new[] { "UserId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelAtStations",
                table: "FuelAtStations",
                columns: new[] { "FuelTypeId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                columns: new[] { "UserId", "FuelStationId" });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rate = table.Column<int>(type: "int", precision: 1, scale: 0, nullable: false),
                    Content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FuelStationId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FuelStationId",
                table: "Reviews",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_FuelStations_FuelStationId",
                table: "Favorites",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelAtStations_FuelStations_FuelStationId",
                table: "FuelAtStations",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelAtStations_FuelTypes_FuelTypeId",
                table: "FuelAtStations",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedStations_FuelStations_FuelStationId",
                table: "OwnedStations",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedStations_Users_UserId",
                table: "OwnedStations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAtStations_FuelStations_FuelStationId",
                table: "ServiceAtStations",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAtStations_Services_ServiceId",
                table: "ServiceAtStations",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_FuelStations_FuelStationId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelAtStations_FuelStations_FuelStationId",
                table: "FuelAtStations");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelAtStations_FuelTypes_FuelTypeId",
                table: "FuelAtStations");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedStations_FuelStations_FuelStationId",
                table: "OwnedStations");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedStations_Users_UserId",
                table: "OwnedStations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAtStations_FuelStations_FuelStationId",
                table: "ServiceAtStations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAtStations_Services_ServiceId",
                table: "ServiceAtStations");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAtStations",
                table: "ServiceAtStations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedStations",
                table: "OwnedStations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelAtStations",
                table: "FuelAtStations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.RenameTable(
                name: "ServiceAtStations",
                newName: "ServiceAtStation");

            migrationBuilder.RenameTable(
                name: "OwnedStations",
                newName: "OwnedStation");

            migrationBuilder.RenameTable(
                name: "FuelAtStations",
                newName: "FuelAtStation");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "Favorite");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceAtStations_FuelStationId",
                table: "ServiceAtStation",
                newName: "IX_ServiceAtStation_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedStations_FuelStationId",
                table: "OwnedStation",
                newName: "IX_OwnedStation_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelAtStations_FuelStationId",
                table: "FuelAtStation",
                newName: "IX_FuelAtStation_FuelStationId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_FuelStationId",
                table: "Favorite",
                newName: "IX_Favorite_FuelStationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAtStation",
                table: "ServiceAtStation",
                columns: new[] { "ServiceId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedStation",
                table: "OwnedStation",
                columns: new[] { "UserId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelAtStation",
                table: "FuelAtStation",
                columns: new[] { "FuelTypeId", "FuelStationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite",
                columns: new[] { "UserId", "FuelStationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_FuelStations_FuelStationId",
                table: "Favorite",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Users_UserId",
                table: "Favorite",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelAtStation_FuelStations_FuelStationId",
                table: "FuelAtStation",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelAtStation_FuelTypes_FuelTypeId",
                table: "FuelAtStation",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedStation_FuelStations_FuelStationId",
                table: "OwnedStation",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedStation_Users_UserId",
                table: "OwnedStation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAtStation_FuelStations_FuelStationId",
                table: "ServiceAtStation",
                column: "FuelStationId",
                principalTable: "FuelStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAtStation_Services_ServiceId",
                table: "ServiceAtStation",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
