using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories {
    
    public class UserRepository : EntityBaseRepository<User>, IUserRepository {
        
        private readonly CourseProjectContext _context;
        public UserRepository (CourseProjectContext context) : base (context)
        {
            _context = context;
        }

        public bool IsEmailUniq (string email) {
            var user = GetSingle(u => u.Email == email);
            return user == null;
        }

        public IQueryable GetAllUsers()
        {
            return (from users in _context.Users
                select new
                {
                    users.Id,
                    users.Email,
                    users.Username,
                    users.LastLoginDate,
                    users.RegistrationDate,
                    users.IsActive,
                    Role = (from roles in _context.Roles
                        where roles.Id == users.Role.Id
                        select roles.Name).SingleOrDefault()

                });
        }

        public bool IsUsernameUniq (string username) {
            var user = GetSingle(u => u.Username == username);
            return user == null;
        }
        
        
    }
}