using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseProject.Model.Entities;

namespace CourseProject.Data
{
    public class CourseProjectContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CrowdfundingCompany> CrowdfundingCompanies { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<CompanyBenefit> CompanyBenefits { get; set; }
        public DbSet<UserBenefit> UserBenefits { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<CompanyNews>  CompanyNews { get; set; }
        public DbSet<CompanyRating> CompanyRating { get; set; }
        public DbSet<LikesOrDislikes> LikesOrDislikes { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<CompanyTags> CompanyTags { get; set; }

        public CourseProjectContext(DbContextOptions<CourseProjectContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            ConfigureModelBuilderForUser(modelBuilder);
            ConfigureModelBuilderForCrowdfundingCompany(modelBuilder);
            ConfigureModelBuilderForVideos(modelBuilder);
            ConfigureModelBuilderForPhotos(modelBuilder);
            ConfigureModelBuilderForPayments(modelBuilder);
            ConfigureModelBuilderForCompanyBenefit(modelBuilder);
            ConfigureModelBuilderForUserBenefit(modelBuilder);
            ConfigureModelBuilderForComments(modelBuilder);
            ConfigureModelBuilderForCompanyNews(modelBuilder);
            ConfigureModelBuilderForCompanyRating(modelBuilder);
            ConfigureModelBuilderForLikesOrDislikes(modelBuilder);
            ConfigureModelBuilderForRoles(modelBuilder);
            ConfigureModelBuilderForTags(modelBuilder);
            ConfigureModelBuilderForCompanyTags(modelBuilder);

        }

        private void ConfigureModelBuilderForCompanyTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyTags>().ToTable("CompanyTags");
        }

        private void ConfigureModelBuilderForTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tags>().ToTable("Tags");
        }

        private void ConfigureModelBuilderForRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Roles");
        }

        private void ConfigureModelBuilderForLikesOrDislikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikesOrDislikes>().ToTable("LikesOrDislikes");
        }

        private void ConfigureModelBuilderForCompanyRating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyRating>().ToTable("CompanyRating");
        }

        private void ConfigureModelBuilderForCompanyNews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyNews>().ToTable("CompanyNews");
        }

        private void ConfigureModelBuilderForComments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comments>().ToTable("Comments");
        }

        private void ConfigureModelBuilderForUserBenefit(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBenefit>().ToTable("UserBenefit");
        }

        private void ConfigureModelBuilderForCompanyBenefit(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyBenefit>().ToTable("CompanyBenefit");
        }

        private void ConfigureModelBuilderForPayments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payments>().ToTable("Payments");
            modelBuilder.Entity<Payments>()
                .Property(payment => payment.Amount)
                .HasPrecision(18, 2);
        }
        
        private void ConfigureModelBuilderForPhotos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photos>().ToTable("Photos");
        }

        private void ConfigureModelBuilderForVideos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Videos>().ToTable("Videos");
        }

        private void ConfigureModelBuilderForCrowdfundingCompany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrowdfundingCompany>().ToTable("CrowdfundingCompany");
            modelBuilder.Entity<CrowdfundingCompany>()
                .Property(company => company.RequiredAmount)
                .HasPrecision(18, 2);
        }

        private void ConfigureModelBuilderForUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<User>()
                .Property(user => user.DesignTheme)
                .HasMaxLength(10)
                .IsRequired()
                .HasDefaultValue("light");
            
            modelBuilder.Entity<User>()
                .Property(user => user.Language)
                .HasMaxLength(4)
                .IsRequired()
                .HasDefaultValue("ru");
            
            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();
        }
    }
}

