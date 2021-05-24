using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TagsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTags_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyTags_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTags_CrowdfundingCompanyId",
                table: "CompanyTags",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTags_TagsId",
                table: "CompanyTags",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
