// <auto-generated />
using System;
using CourseProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CourseProject.Api.Migrations
{
    [DbContext(typeof(CourseProjectContext))]
    [Migration("20210425091306_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.3.21201.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CourseProject.Model.Entities.Comments", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyBenefit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Cost")
                        .HasColumnType("real");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.ToTable("CompanyBenefit");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyNews", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.ToTable("CompanyNews");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyRating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("CompanyRating");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CrowdfundingCompany", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndCompanyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequiredAmount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CrowdfundingCompany");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.LikesOrDislikes", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CommentsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsLike")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CommentsId");

                    b.HasIndex("UserId");

                    b.ToTable("LikesOrDislikes");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Payments", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Photos", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DesignTheme")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValue("light");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)")
                        .HasDefaultValue("ru");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.UserBenefit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyBenefitId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyBenefitId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBenefit");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Videos", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CrowdfundingCompanyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CrowdfundingCompanyId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Comments", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyBenefit", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyNews", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CompanyRating", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.CrowdfundingCompany", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.LikesOrDislikes", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.Comments", "Comments")
                        .WithMany()
                        .HasForeignKey("CommentsId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Comments");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Payments", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Photos", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.UserBenefit", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CompanyBenefit", "CompanyBenefit")
                        .WithMany()
                        .HasForeignKey("CompanyBenefitId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CourseProject.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CompanyBenefit");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseProject.Model.Entities.Videos", b =>
                {
                    b.HasOne("CourseProject.Model.Entities.CrowdfundingCompany", "CrowdfundingCompany")
                        .WithMany()
                        .HasForeignKey("CrowdfundingCompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CrowdfundingCompany");
                });
#pragma warning restore 612, 618
        }
    }
}
