namespace CourseProject.Model.Entities
{
    public class LikesOrDislikes : IEntityBase
    {
        public string Id { get; set; }
        public bool IsLike { get; set; }
        public User User { get; set; }
        public Comments Comments { get; set; }
    }
}