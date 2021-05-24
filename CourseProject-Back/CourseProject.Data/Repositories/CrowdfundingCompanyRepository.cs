using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data.Repositories
{
    public class CrowdfundingCompanyRepository : EntityBaseRepository<CrowdfundingCompany>,
        ICrowdfundingCompanyRepository
    {
        private readonly CourseProjectContext _context;

        public CrowdfundingCompanyRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllCompanies()
        {
            return (from company in _context.CrowdfundingCompanies
                select new
                {
                    company.Id,
                    company.Title,
                    company.Theme,
                    company.RequiredAmount,
                    company.EndCompanyDate,
                    company.MainPhotoUrl,
                    company.Description,
                    СollectedNow = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == company.Id
                        select payments.Amount).DefaultIfEmpty().Sum(),
                    СompletionPercentage = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == company.Id
                        select payments.Amount).DefaultIfEmpty().Sum() / (company.RequiredAmount / 100M),
                    Rating = (from rating in _context.CompanyRating
                        where rating.CrowdfundingCompany.Id == company.Id
                        select rating.Rating).DefaultIfEmpty().Average(),
                    Tags = (from companyTags in _context.CompanyTags
                        where companyTags.CrowdfundingCompany.Id == company.Id
                        join tags in _context.Tags on companyTags.Tags.Id equals tags.Id
                        select tags.Name).ToList()
                });
        }

        public IQueryable GetSingleCompany(string id)
        {
            return (from company in _context.CrowdfundingCompanies
                where company.Id == id
                select new
                {
                    company.Id,
                    company.Title,
                    company.Theme,
                    company.RequiredAmount,
                    company.EndCompanyDate,
                    company.MainPhotoUrl,
                    company.Description,
                    СollectedNow = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == id
                        select payments.Amount).DefaultIfEmpty().Sum(),
                    СompletionPercentage = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == company.Id
                        select payments.Amount).DefaultIfEmpty().Sum() / (company.RequiredAmount / 100M),
                    Rating = (from rating in _context.CompanyRating
                        where rating.CrowdfundingCompany.Id == id
                        select rating.Rating).DefaultIfEmpty().Average(),
                    Tags = (from companyTags in _context.CompanyTags
                        where companyTags.CrowdfundingCompany.Id == id
                        join tags in _context.Tags on companyTags.Tags.Id equals tags.Id
                        select tags.Name).AsSingleQuery().ToList(),
                    Owner = (from user in _context.Users
                        where user.Id == company.User.Id
                        select user.Id).SingleOrDefault(),
                    videoUrl = (from video in _context.Videos
                        where video.CrowdfundingCompany.Id == id
                        select video.VideoUrl).SingleOrDefault(),
                    Photos = (from photos in _context.Photos
                        where photos.CrowdfundingCompany.Id == id
                        select new {photos.Id, photos.PhotoUrl}).AsSingleQuery().DefaultIfEmpty().ToList()
                });
        }

        public bool CheckIfHasAccess(string userId, string companyId)
        {
            bool hasAccessByOwnerId = (from company in _context.CrowdfundingCompanies
                where company.User.Id == userId && company.Id == companyId
                select company).Any();
            bool hasAccessByRole = (from user in _context.Users
                where user.Id == userId && user.Role.Name == "Admin"
                select user).Any();
            return hasAccessByOwnerId || hasAccessByRole;
        }

        public IQueryable GetUserCompanies(string userId)
        {
            return (from company in _context.CrowdfundingCompanies
                where company.User.Id == userId
                select new
                {
                    company.Id,
                    company.Title,
                    company.Theme,
                    company.RequiredAmount,
                    company.EndCompanyDate,
                    company.MainPhotoUrl,
                    company.Description,
                    СollectedNow = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == company.Id
                        select payments.Amount).DefaultIfEmpty().Sum(),
                    СompletionPercentage = (from payments in _context.Payments
                        where payments.CrowdfundingCompany.Id == company.Id
                        select payments.Amount).DefaultIfEmpty().Sum() / (company.RequiredAmount / 100M),
                    Rating = (from rating in _context.CompanyRating
                        where rating.CrowdfundingCompany.Id == company.Id
                        select rating.Rating).DefaultIfEmpty().Average(),
                    Tags = (from companyTags in _context.CompanyTags
                        where companyTags.CrowdfundingCompany.Id == company.Id
                        join tags in _context.Tags on companyTags.Tags.Id equals tags.Id
                        select tags.Name).ToList()
                });
        }
    }
}