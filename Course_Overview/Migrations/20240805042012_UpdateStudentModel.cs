using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Overview.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FailedAttempts",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNewUser",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEnd",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiration",
                table: "Students",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FailedAttempts",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsNewUser",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiration",
                table: "Students");
        }
    }
}
