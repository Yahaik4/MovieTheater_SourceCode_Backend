using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaService.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateReportRevenueTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyRevenueReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CinemaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sales = table.Column<decimal>(type: "numeric", nullable: false),
                    TicketSold = table.Column<int>(type: "integer", nullable: false),
                    ProjectedProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRevenueReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRevenueReports_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodDrinkRevenueReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CinemaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodDrinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sales = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ProjectedProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodDrinkRevenueReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodDrinkRevenueReports_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodDrinkRevenueReports_FoodDrink_FoodDrinkId",
                        column: x => x.FoodDrinkId,
                        principalTable: "FoodDrink",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieRevenueReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CinemaId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovieId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sales = table.Column<decimal>(type: "numeric", nullable: false),
                    TicketSold = table.Column<int>(type: "integer", nullable: false),
                    ProjectedProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieRevenueReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieRevenueReports_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRevenueReports_CinemaId",
                table: "DailyRevenueReports",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDrinkRevenueReports_CinemaId",
                table: "FoodDrinkRevenueReports",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDrinkRevenueReports_FoodDrinkId",
                table: "FoodDrinkRevenueReports",
                column: "FoodDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieRevenueReports_CinemaId",
                table: "MovieRevenueReports",
                column: "CinemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRevenueReports");

            migrationBuilder.DropTable(
                name: "FoodDrinkRevenueReports");

            migrationBuilder.DropTable(
                name: "MovieRevenueReports");
        }
    }
}
