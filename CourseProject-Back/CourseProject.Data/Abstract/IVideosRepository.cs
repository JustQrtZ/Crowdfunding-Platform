using System.Collections.Generic;
using System.Linq;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface IVideosRepository : IEntityBaseRepository<Videos>
    {
        public IQueryable GetVideoForCompany(string id);
    }
}