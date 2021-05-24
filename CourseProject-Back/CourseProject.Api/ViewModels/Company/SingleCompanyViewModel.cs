using System;
using System.Collections.Generic;
using CourseProject.Model.Entities;

namespace CourseProject.Api.ViewModels.Company
{
    public class SingleCompanyViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public decimal RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime EndCompanyDate { get; set;}
        public int СollectedNow { get; set; }
        public float СompletionPercentage { get; set; }
        public float Rating { get; set; }
        public string[] Tags { get; set; }
        public  string Owner { get; set; }
    }
}