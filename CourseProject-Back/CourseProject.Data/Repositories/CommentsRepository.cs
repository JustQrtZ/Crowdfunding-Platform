using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CommentsRepository : EntityBaseRepository<Comments>, ICommentsRepository
    {
        private readonly CourseProjectContext _context;

        public CommentsRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable AllCompanyComments(string companyId)
        {
            return (from comments in _context.Comments
                where comments.Company.Id == companyId
                orderby comments.CreationDate
                select new
                {
                    comments.Id, comments.Content, User = comments.User.Username, comments.CreationDate,
                    LikeCount = (from like in _context.LikesOrDislikes
                            where like.Comments.Id == comments.Id
                            group like by like.Comments.Id into g
                            select g.Sum(x => x.IsLike ? 1 : -1)).SingleOrDefault()
                });
        }
    }
}