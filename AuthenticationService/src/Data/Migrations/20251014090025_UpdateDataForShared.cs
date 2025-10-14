using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataForShared : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Sessions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Sessions",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Sessions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Sessions",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sessions",
                newName: "SessionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
