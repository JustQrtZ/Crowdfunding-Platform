using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CompanyNews_CompanyNewsId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CompanyNewsId",
                table: "Comments",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CompanyNewsId",
                table: "Comments",
                newName: "IX_Comments_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CrowdfundingCompany_CompanyId",
                table: "Comments",
                column: "CompanyId",
                principalTable: "CrowdfundingCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CrowdfundingCompany_CompanyId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Comments",
                newName: "CompanyNewsId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CompanyId",
                table: "Comments",
                newName: "IX_Comments_CompanyNewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CompanyNews_CompanyNewsId",
                table: "Comments",
                column: "CompanyNewsId",
                principalTable: "CompanyNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
