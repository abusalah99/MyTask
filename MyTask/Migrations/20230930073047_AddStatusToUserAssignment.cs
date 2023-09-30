using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTask.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToUserAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UserAssignment",
                type: "text",
                nullable: false,
                defaultValue: "Unfinished");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserAssignment");
        }
    }
}
