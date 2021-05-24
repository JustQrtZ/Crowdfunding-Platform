using System.Collections.Generic;
using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CompanyBenefitRepository : EntityBaseRepository<CompanyBenefit>, ICompanyBenefitRepository {
        public CompanyBenefitRepository (CourseProjectContext context) : base (context) { }
        public List<CompanyBenefit> GetAllBenefitsForCompany(string companyId)
        {
            return FindBy(x => x.CrowdfundingCompany.Id == companyId).ToList();
        }
    }
}