using System;

namespace CourseProject.Model.Entities
{
    public class CompanyNews : IEntityBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
    }
}