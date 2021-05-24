namespace CourseProject.Api.ViewModels.Comments
{
    public class CreateCommentForCompanyNewsViewModel
    {
        public string User { get; set; }
        public string Content { get; set; }
        public string CompanyNews { get; set; }
        public string CreationDate { get; set; }
    }
}