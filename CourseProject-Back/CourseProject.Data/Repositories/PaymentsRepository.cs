using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class PaymentsRepository : EntityBaseRepository<Payments>, IPaymentsRepository
    {
        public PaymentsRepository(CourseProjectContext context) : base(context)
        {
        }
    }
}