using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddFKtoTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserusrId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserusrId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserusrId",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_usrId",
                table: "Tasks",
                column: "usrId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_usrId",
                table: "Tasks",
                column: "usrId",
                principalTable: "Users",
                principalColumn: "usrId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_usrId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_usrId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "UserusrId",
                table: "Tasks",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserusrId",
                table: "Tasks",
                column: "UserusrId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserusrId",
                table: "Tasks",
                column: "UserusrId",
                principalTable: "Users",
                principalColumn: "usrId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
