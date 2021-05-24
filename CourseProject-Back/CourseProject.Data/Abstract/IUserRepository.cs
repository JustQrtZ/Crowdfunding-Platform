using System.Linq;
using System.Threading.Tasks;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        bool IsUsernameUniq(string username);
        bool IsEmailUniq(string email);

        public IQueryable GetAllUsers();
    }
}