namespace CourseProject.Api.ViewModels.Comments
{
    public class CreateLikeOrDislikeViewModel
    {
        public string User { get; set; }
        public string Comment { get; set; }
        public bool LikeOrDislike { get; set; }
        public string Company { get; set; }
    }
}