using System;

namespace CourseProject.Model.Entities
{
    public class Payments : IEntityBase
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public CrowdfundingCompany CrowdfundingCompany { get; set; }
        public CompanyBenefit CompanyBenefit { get; set; }
        public User User { get; set; }
    }
}