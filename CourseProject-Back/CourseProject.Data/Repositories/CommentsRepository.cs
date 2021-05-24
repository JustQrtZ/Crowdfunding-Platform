using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CommentsRepository : EntityBaseRepository<Comments>, ICommentsRepository {
        public CommentsRepository (CourseProjectContext context) : base (context) { }
    }
}