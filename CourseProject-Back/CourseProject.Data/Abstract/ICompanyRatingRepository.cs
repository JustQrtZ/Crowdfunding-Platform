using System.Linq;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ICompanyRatingRepository : IEntityBaseRepository<CompanyRating>
    {
        public float GetUserCompanyRating(string userId, string companyId);
    }
}