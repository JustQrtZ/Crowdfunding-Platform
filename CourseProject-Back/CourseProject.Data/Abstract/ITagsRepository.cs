using CourseProject.Model.Entities;

namespace CourseProject.Data.Abstract
{
    public interface ITagsRepository : IEntityBaseRepository<Tags>
    {
        bool IsTagUniq(string tag);
    }
}