using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationShipCustomerTypeAndPriceRuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceRules_CustomerTypeId",
                table: "PriceRules");

            migrationBuilder.CreateIndex(
                name: "IX_PriceRules_CustomerTypeId",
                table: "PriceRules",
                column: "CustomerTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceRules_CustomerTypeId",
                table: "PriceRules");

            migrationBuilder.CreateIndex(
                name: "IX_PriceRules_CustomerTypeId",
                table: "PriceRules",
                column: "CustomerTypeId",
                unique: true);
        }
    }
}
