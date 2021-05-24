using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class TagsRepository : EntityBaseRepository<Tags>, ITagsRepository
    {
        public TagsRepository(CourseProjectContext context) : base(context) { }

        public bool IsTagUniq(string tag)
        {
            var tags = GetSingle(t => t.Name == tag);
            return tags == null;
        }
    }
}