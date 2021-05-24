using System.Linq;
using CourseProject.Model;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface IUserBenefitRepository : IEntityBaseRepository<UserBenefit>
    {
        public IQueryable GetUserBenefits(string userId);
    }
}