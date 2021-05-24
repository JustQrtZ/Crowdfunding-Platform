using System;

namespace CourseProject.Api.ViewModels.Company
{
    public class EditCompanyViewModel
    {
        public string CompanyId { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public int RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public string EndCompanyDate { get; set; }
        
        public string VideoUrl { get; set; }
        
        public string[] Photos { get; set; }

    }

    public class DeleteCompanyViewModel
    {
        public string CompanyId { get; set; }
    }
}