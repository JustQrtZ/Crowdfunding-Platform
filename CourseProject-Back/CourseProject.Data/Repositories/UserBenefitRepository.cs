using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class UserBenefitRepository : EntityBaseRepository<UserBenefit>, IUserBenefitRepository
    {
        private readonly CourseProjectContext _context;

        public UserBenefitRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetUserBenefits(string userId)
        {
            return (from userBenefit in _context.UserBenefits
                where userBenefit.User.Id == userId
                select new
                {
                    userBenefit.Id,
                    Name = (from benefit in _context.CompanyBenefits
                        where benefit.Id == userBenefit.CompanyBenefit.Id
                        select benefit.Name).SingleOrDefault(),
                    Cost = (from benefit in _context.CompanyBenefits
                        where benefit.Id == userBenefit.CompanyBenefit.Id
                        select benefit.Cost).SingleOrDefault()
                   
                });
        }
    }
}