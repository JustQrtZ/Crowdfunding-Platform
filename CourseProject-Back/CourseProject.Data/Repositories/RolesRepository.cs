using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class RolesRepository : EntityBaseRepository<Role>, IRolesRepository {
        public RolesRepository (CourseProjectContext context) : base (context) { }
    }
}