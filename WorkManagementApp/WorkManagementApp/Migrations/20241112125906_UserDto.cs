using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class UserDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Projects",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedUserId",
                table: "Projects",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_AssignedUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AssignedUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Projects");
        }
    }
}
