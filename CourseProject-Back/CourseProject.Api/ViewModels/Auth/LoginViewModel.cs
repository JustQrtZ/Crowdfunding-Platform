using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Api.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required] 
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        public DateTime LastLoginDate { get; set; }
    }

    public class GoogleLodInViewModel
    {
        [Required]
        public string TokenId {get; set;}
    }
    
    public class FacebookLodInViewModel
    {
        [Required]
        public string Token {get; set;}
    }

    public class FacebookUserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}