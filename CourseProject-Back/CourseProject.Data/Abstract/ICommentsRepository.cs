using System.Linq;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ICommentsRepository : IEntityBaseRepository<Comments>
    {
        public IQueryable AllCompanyComments(string companyId);
    }
}