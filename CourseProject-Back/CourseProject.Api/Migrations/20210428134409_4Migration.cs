using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class _4Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "CrowdfundingCompany",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "CrowdfundingCompany");
        }
    }
}
