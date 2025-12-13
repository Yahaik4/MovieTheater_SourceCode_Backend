using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndDescriptionToFoodDrink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoodDrink",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "FoodDrink",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoodDrink");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "FoodDrink");
        }
    }
}
