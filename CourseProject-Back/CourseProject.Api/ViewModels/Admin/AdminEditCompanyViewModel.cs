namespace CourseProject.Api.ViewModels.Admin
{
    public class AdminEditCompanyViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public int RequiredAmount { get; set; }
        public string MainPhotoUrl { get; set; }
        public string EndCompanyDate { get; set; }
        public string UserId { get; set; }
    }
}