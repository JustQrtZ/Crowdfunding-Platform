namespace CourseProject.Model.Entities
{
    public class Videos : IEntityBase
    {
        public string Id { get; set; }
        public string VideoUrl { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
    }
}