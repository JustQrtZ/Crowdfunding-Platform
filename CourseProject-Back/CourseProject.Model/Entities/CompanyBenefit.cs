namespace CourseProject.Model.Entities
{
    public class CompanyBenefit : IEntityBase
    {
        public string Id { get; set; }
        public float Cost { get; set; }
        public string Name { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
    }
}