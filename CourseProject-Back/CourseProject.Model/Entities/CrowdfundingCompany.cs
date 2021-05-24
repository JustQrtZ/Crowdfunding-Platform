using System;

namespace CourseProject.Model.Entities
{
    public class CrowdfundingCompany : IEntityBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public decimal RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime EndCompanyDate { get; set;}
        public User User { get; set; }
    }
}