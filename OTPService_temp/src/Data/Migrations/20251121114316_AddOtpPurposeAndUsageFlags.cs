using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTPService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpPurposeAndUsageFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "OTPs",
                type: "text",
                nullable: false,
                defaultValue: "register");

            migrationBuilder.CreateIndex(
                name: "IX_OTPs_UserId_Purpose",
                table: "OTPs",
                columns: new[] { "UserId", "Purpose" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OTPs_UserId_Purpose",
                table: "OTPs");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "OTPs");

            migrationBuilder.CreateIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs",
                column: "UserId",
                unique: true);
        }
    }
}
