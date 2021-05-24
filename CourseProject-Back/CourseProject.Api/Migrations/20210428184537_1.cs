using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyBenefitId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrowdfundingCompanyId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyBenefitId",
                table: "Payments",
                column: "CompanyBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CrowdfundingCompanyId",
                table: "Payments",
                column: "CrowdfundingCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CompanyBenefit_CompanyBenefitId",
                table: "Payments",
                column: "CompanyBenefitId",
                principalTable: "CompanyBenefit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CrowdfundingCompany_CrowdfundingCompanyId",
                table: "Payments",
                column: "CrowdfundingCompanyId",
                principalTable: "CrowdfundingCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CompanyBenefit_CompanyBenefitId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CrowdfundingCompany_CrowdfundingCompanyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CompanyBenefitId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CrowdfundingCompanyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CompanyBenefitId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CrowdfundingCompanyId",
                table: "Payments");
        }
    }
}
