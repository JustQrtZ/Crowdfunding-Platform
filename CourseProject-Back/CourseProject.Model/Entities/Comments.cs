using System;

namespace CourseProject.Model.Entities
{
    public class Comments : IEntityBase
    {
        public string Id { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public CompanyNews CompanyNews { get; set; }
        public DateTime CreationDate { get; set; }
    }
}