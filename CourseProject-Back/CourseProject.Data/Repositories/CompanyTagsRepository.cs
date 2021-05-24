using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class CompanyTagsRepository : EntityBaseRepository<CompanyTags>, ICompanyTagsRepository
    {
        public CompanyTagsRepository(CourseProjectContext context) : base(context) { }
        
    }
}