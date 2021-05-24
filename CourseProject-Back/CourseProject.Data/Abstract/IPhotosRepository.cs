using System.Collections.Generic;
using System.Linq;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface IPhotosRepository : IEntityBaseRepository<Photos>
    {
        public IQueryable GetAllPhotosForCompany(string id);
    }
}