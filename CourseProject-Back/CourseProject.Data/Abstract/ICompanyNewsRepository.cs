using System.Collections.Generic;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ICompanyNewsRepository : IEntityBaseRepository<CompanyNews>
    {
        public List<CompanyNews> GetAllNewsForCompany(string companyId);
    }
}