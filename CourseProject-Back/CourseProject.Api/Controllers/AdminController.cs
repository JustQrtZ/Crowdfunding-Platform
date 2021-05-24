using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Admin;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ICrowdfundingCompanyRepository _crowdfundingCompany;
        private readonly IRolesRepository _roles;

        public AdminController(IUserRepository userRepository, ITokenService tokenService,
            ICrowdfundingCompanyRepository crowdfundingCompany, IRolesRepository roles)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _crowdfundingCompany = crowdfundingCompany;
            _roles = roles;
        }

        [HttpGet("getAllUsers")]
        public IQueryable GetAllUsers()
        {
            var allUsers = _userRepository.GetAllUsers();
            return allUsers;
        }

        [HttpPost("blockUsers")]
        public ActionResult BlockUsers([FromBody] string[] usersId)
        {
            var usersListToBlock = _userRepository.FindBy(user => usersId.Contains(user.Id));
            foreach (var user in usersListToBlock)
            {
                user.IsActive = false;
                user.RefreshToken = null;
                _userRepository.Update(user);
            }

            _userRepository.Commit();

            return Ok();
        }
        
        [HttpPatch("makeAdmin")]
        public ActionResult MakeAdmin([FromBody] string[] usersId)
        {
            var usersListToMakeAdmins = _userRepository.FindBy(user => usersId.Contains(user.Id));
            var adminRole = _roles.GetSingle(x => x.Name == "Admin");
            foreach (var user in usersListToMakeAdmins)
            {
                user.Role = adminRole;
                _userRepository.Update(user);
            }
            _userRepository.Commit();
            return Ok();
        }
        
        [HttpPatch("makeUser")]
        public ActionResult MakeUser([FromBody] string[] usersId)
        {
            var usersListToMakeAdmins = _userRepository.FindBy(user => usersId.Contains(user.Id));
            var userRole = _roles.GetSingle(x => x.Name == "User");
            foreach (var user in usersListToMakeAdmins)
            {
                user.Role = userRole;
                _userRepository.Update(user);
            }
            _userRepository.Commit();
            return Ok();
        }

        [HttpPost("unBlockUsers")]
        public ActionResult UnBlockUsers([FromBody] string[] usersId)
        {
            var usersListToUnBlock = _userRepository.FindBy(user => usersId.Contains(user.Id));
            foreach (var user in usersListToUnBlock)
            {
                user.IsActive = true;
                _userRepository.Update(user);
            }

            _userRepository.Commit();
            return Ok();
        }

        [HttpPost("deleteUsers")]
        public ActionResult DeleteUsers([FromBody] string[] usersId)
        {
            _userRepository.DeleteWhere(user => usersId.Contains(user.Id));
            _userRepository.Commit();
            return Ok();
        }

        [HttpPost("getUserCompany/{id}")]
        [Authorize]
        public ActionResult<List<CrowdfundingCompany>> GetUserCompany(string id)
        {
            return _crowdfundingCompany.FindBy(x => x.User.Id == id).ToList();
        }

        [HttpPost("editUserCompany")]
        public ActionResult EditUserCompany([FromBody] AdminEditCompanyViewModel companyViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var company =
                _crowdfundingCompany.GetSingle(x => x.Id == companyViewModel.Id && x.User.Id == companyViewModel.UserId,
                    x => x.User);
            if (company == null) return BadRequest(new {company = "company does not exist or userid does not exist"});

            company.Title = companyViewModel.Title;
            company.Description = companyViewModel.Description;
            company.RequiredAmount = companyViewModel.RequiredAmount;
            company.MainPhotoUrl = companyViewModel.MainPhotoUrl;
            company.EndCompanyDate = DateTime.Parse(companyViewModel.EndCompanyDate, CultureInfo.InvariantCulture);
            company.Theme = companyViewModel.Theme;

            _crowdfundingCompany.Update(company);
            _crowdfundingCompany.Commit();
            return Ok(company);
        }

        [HttpPost("getLoginInUser")]
        public ActionResult GetLoginInUser()
        {
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers["Authorization"].ToString()
                    .Replace("Bearer ", ""));
            var user = _userRepository.GetSingle(u => u.Email == principal.Identity.Name);
            if (user == null) return BadRequest("User not found");

            return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.Username
                }
            );
        }
    }
}