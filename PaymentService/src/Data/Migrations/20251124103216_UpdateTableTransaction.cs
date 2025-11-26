using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Transactions",
                newName: "TxnRef");

            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "Transactions",
                newName: "PaymentMethodType");

            migrationBuilder.AddColumn<string>(
                name: "PaymentGateway",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayTransactionNo",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderMeta",
                table: "Transactions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentGateway",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayTransactionNo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ProviderMeta",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TxnRef",
                table: "Transactions",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodType",
                table: "Transactions",
                newName: "PaymentIntentId");
        }
    }
}
