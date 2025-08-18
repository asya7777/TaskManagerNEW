using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class SetVerifiedDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "verified",
                table: "Users",
                type: "NUMBER(1)",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "NUMBER(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_email",
                table: "Users");

            migrationBuilder.AlterColumn<bool>(
                name: "verified",
                table: "Users",
                type: "NUMBER(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "NUMBER(1)",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");
        }
    }
}
