using System;

namespace CourseProject.Api.ViewModels.Users
{
    public class UserViewModel
    {
        public UserViewModel(string id, string email, string username, DateTime registrationDate,
            DateTime lastLoginDate, bool isActive)
        {
            Id = id;
            Email = email;
            Username = username;
            RegistrationDate = registrationDate;
            LastLoginDate = lastLoginDate;
            IsActive = isActive;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public bool IsActive { get; set; }
    }
}