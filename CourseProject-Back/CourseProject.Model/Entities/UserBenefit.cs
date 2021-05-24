namespace CourseProject.Model.Entities
{
    public class UserBenefit : IEntityBase
    {
        public string Id { get; set; }
        public User User { get; set; }
        public CompanyBenefit CompanyBenefit { get; set; }
    }
}