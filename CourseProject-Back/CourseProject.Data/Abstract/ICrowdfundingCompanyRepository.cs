using System.Linq;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ICrowdfundingCompanyRepository : IEntityBaseRepository<CrowdfundingCompany>
    {
        public IQueryable GetAllCompanies();
        public IQueryable GetSingleCompany(string id);
        public bool CheckIfHasAccess(string userId, string companyId);

        public IQueryable GetUserCompanies(string userId);

    }
}