using System;
using System.Collections.Generic;
using CinemaService.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaService.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookinngId",
                table: "ShowtimeSeats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShowtimeSeats",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "integer", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    BookingSeats = table.Column<List<BookingSeat>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimeSeats_BookinngId",
                table: "ShowtimeSeats",
                column: "BookinngId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookinngId",
                table: "ShowtimeSeats",
                column: "BookinngId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookinngId",
                table: "ShowtimeSeats");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_ShowtimeSeats_BookinngId",
                table: "ShowtimeSeats");

            migrationBuilder.DropColumn(
                name: "BookinngId",
                table: "ShowtimeSeats");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShowtimeSeats");
        }
    }
}
