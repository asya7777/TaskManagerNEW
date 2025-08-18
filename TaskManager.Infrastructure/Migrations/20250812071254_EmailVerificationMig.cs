using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerificationMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Users",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Users",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "verificationToken",
                table: "Users",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "verificationTokenExpiration",
                table: "Users",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "verified",
                table: "Users",
                type: "NUMBER(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "verifiedAt",
                table: "Users",
                type: "TIMESTAMP(7)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "verificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "verificationTokenExpiration",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "verified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "verifiedAt",
                table: "Users");
        }
    }
}
