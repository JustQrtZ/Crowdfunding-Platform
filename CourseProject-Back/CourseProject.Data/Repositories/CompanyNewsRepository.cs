using System.Collections.Generic;
using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CompanyNewsRepository : EntityBaseRepository<CompanyNews>, ICompanyNewsRepository {
        public CompanyNewsRepository (CourseProjectContext context) : base (context) { }

        public List<CompanyNews> GetAllNewsForCompany(string companyId)
        {
            return FindBy(x => x.CrowdfundingCompany.Id == companyId).ToList();
        }
    }
}