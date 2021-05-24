namespace CourseProject.Model.Entities
{
    public class Photos : IEntityBase
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
    }
}