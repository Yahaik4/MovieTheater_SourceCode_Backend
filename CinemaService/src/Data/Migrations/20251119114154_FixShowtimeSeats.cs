using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaService.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixShowtimeSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookinngId",
                table: "ShowtimeSeats");

            migrationBuilder.RenameColumn(
                name: "BookinngId",
                table: "ShowtimeSeats",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_ShowtimeSeats_BookinngId",
                table: "ShowtimeSeats",
                newName: "IX_ShowtimeSeats_BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookingId",
                table: "ShowtimeSeats",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookingId",
                table: "ShowtimeSeats");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "ShowtimeSeats",
                newName: "BookinngId");

            migrationBuilder.RenameIndex(
                name: "IX_ShowtimeSeats_BookingId",
                table: "ShowtimeSeats",
                newName: "IX_ShowtimeSeats_BookinngId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowtimeSeats_Bookings_BookinngId",
                table: "ShowtimeSeats",
                column: "BookinngId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
