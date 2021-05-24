using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CrowdfundingCompany_CrowdfundingCompanyId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CrowdfundingCompanyId",
                table: "Comments",
                newName: "CompanyNewsId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CrowdfundingCompanyId",
                table: "Comments",
                newName: "IX_Comments_CompanyNewsId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "RequiredAmount",
                table: "CrowdfundingCompany",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CompanyNews_CompanyNewsId",
                table: "Comments",
                column: "CompanyNewsId",
                principalTable: "CompanyNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CompanyNews_CompanyNewsId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CompanyNewsId",
                table: "Comments",
                newName: "CrowdfundingCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CompanyNewsId",
                table: "Comments",
                newName: "IX_Comments_CrowdfundingCompanyId");

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "Payments",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredAmount",
                table: "CrowdfundingCompany",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CrowdfundingCompany_CrowdfundingCompanyId",
                table: "Comments",
                column: "CrowdfundingCompanyId",
                principalTable: "CrowdfundingCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
