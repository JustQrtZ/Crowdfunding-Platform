using System;

namespace CourseProject.Api.ViewModels.Company
{
    public class AllCompaniesViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public int RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime EndCompanyDate { get; set;}
        public string Owner { get; set; }

        public AllCompaniesViewModel(string id, string title, string theme, string desc, int requiredAmount,
            string mainPhotoUrl, DateTime endCompanyDate, string owner)
        {
            Id = id;
            Title = title;
            Theme = theme;
            Description = desc;
            RequiredAmount = requiredAmount;
            MainPhotoUrl = mainPhotoUrl;
            EndCompanyDate = endCompanyDate;
            Owner = owner;
        }
    }
}