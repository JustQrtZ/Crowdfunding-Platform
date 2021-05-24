namespace CourseProject.Model.Entities
{
    public class CompanyRating : IEntityBase
    {
        public string Id { get; set; }
        public float Rating { get; set; }
        public User User { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
    }
}