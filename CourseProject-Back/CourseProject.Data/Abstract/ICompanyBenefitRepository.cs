using System.Collections.Generic;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ICompanyBenefitRepository : IEntityBaseRepository<CompanyBenefit>
    {
        public List<CompanyBenefit> GetAllBenefitsForCompany(string companyId);
    }
}