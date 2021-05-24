using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class LikesOrDislikesRepository : EntityBaseRepository<LikesOrDislikes>, ILikesOrDislikesRepository
    {
        public LikesOrDislikesRepository(CourseProjectContext context) : base(context)
        {
        }
    }
}