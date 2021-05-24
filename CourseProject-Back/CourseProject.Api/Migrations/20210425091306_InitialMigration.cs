using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseProject.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false, defaultValue: "ru"),
                    DesignTheme = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "light"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdfundingCompany",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredAmount = table.Column<int>(type: "int", nullable: false),
                    MainPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndCompanyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdfundingCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrowdfundingCompany_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBenefit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cost = table.Column<float>(type: "real", nullable: false),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyBenefit_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyNews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyNews_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRating",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRating_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyRating_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrowdfundingCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_CrowdfundingCompany_CrowdfundingCompanyId",
                        column: x => x.CrowdfundingCompanyId,
                        principalTable: "CrowdfundingCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikesOrDislikes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesOrDislikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesOrDislikes_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikesOrDislikes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBenefit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyBenefitId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBenefit_CompanyBenefit_CompanyBenefitId",
                        column: x => x.CompanyBenefitId,
                        principalTable: "CompanyBenefit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBenefit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CrowdfundingCompanyId",
                table: "Comments",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBenefit_CrowdfundingCompanyId",
                table: "CompanyBenefit",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyNews_CrowdfundingCompanyId",
                table: "CompanyNews",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRating_CrowdfundingCompanyId",
                table: "CompanyRating",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRating_UserId",
                table: "CompanyRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrowdfundingCompany_UserId",
                table: "CrowdfundingCompany",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesOrDislikes_CommentsId",
                table: "LikesOrDislikes",
                column: "CommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesOrDislikes_UserId",
                table: "LikesOrDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CrowdfundingCompanyId",
                table: "Photos",
                column: "CrowdfundingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefit_CompanyBenefitId",
                table: "UserBenefit",
                column: "CompanyBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBenefit_UserId",
                table: "UserBenefit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CrowdfundingCompanyId",
                table: "Videos",
                column: "CrowdfundingCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyNews");

            migrationBuilder.DropTable(
                name: "CompanyRating");

            migrationBuilder.DropTable(
                name: "LikesOrDislikes");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "UserBenefit");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CompanyBenefit");

            migrationBuilder.DropTable(
                name: "CrowdfundingCompany");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
