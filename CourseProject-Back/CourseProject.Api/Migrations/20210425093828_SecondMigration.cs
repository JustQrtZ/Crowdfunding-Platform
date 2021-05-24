using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
