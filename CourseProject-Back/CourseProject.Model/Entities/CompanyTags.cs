namespace CourseProject.Model.Entities
{
    public class CompanyTags : IEntityBase
    {
        public string Id { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
        public Tags Tags { get; set; }
    }
}