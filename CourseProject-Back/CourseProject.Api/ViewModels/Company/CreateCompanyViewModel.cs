using System;

namespace CourseProject.Api.ViewModels.Company
{
    public class CreateCompanyViewModel
    {
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public int RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public string EndCompanyDate { get; set; }
        public string[] Tags { get; set; }
        public string UserId { get; set; }
    }
}