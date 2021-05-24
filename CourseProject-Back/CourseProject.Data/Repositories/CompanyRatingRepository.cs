using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CompanyRatingRepository : EntityBaseRepository<CompanyRating>, ICompanyRatingRepository
    {
        private readonly CourseProjectContext _context;

        public CompanyRatingRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public float GetUserCompanyRating(string userId, string companyId)
        {
            return (from rating in _context.CompanyRating
                where rating.User.Id == userId && rating.CrowdfundingCompany.Id == companyId
                select rating.Rating).SingleOrDefault();
        }
    }
}