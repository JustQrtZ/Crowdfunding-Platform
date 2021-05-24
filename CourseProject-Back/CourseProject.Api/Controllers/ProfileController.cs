using System.Linq;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Users;
using CourseProject.Data.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class ProfileController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ICrowdfundingCompanyRepository _company;
        private readonly IUserBenefitRepository _userBenefit;

        public ProfileController(ITokenService tokenService, IUserRepository userRepository,
            ICrowdfundingCompanyRepository company, IUserBenefitRepository userBenefit)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _company = company;
            _userBenefit = userBenefit;
        }

        [HttpGet("getUserProfile")]
        public ActionResult GetProfile()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var user = _userRepository.GetSingle(x => x.Id == userId);
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.DesignTheme,
                user.Language,
                user.LastLoginDate
            });
        }

        [HttpGet("getUserCompanies")]
        public ActionResult GetUserCompanies()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var companies = _company.GetUserCompanies(userId);
            return Ok(companies);
        }

        [HttpGet("getUserBenefits")]
        public ActionResult GetUserBenefits()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var benefits = _userBenefit.GetUserBenefits(userId);
            return Ok(new
            {
                benefits
            });
        }

        [HttpPost("changeUserLanguage")]
        public ActionResult ChangeUserLanguage([FromBody] LanguageViewModel language)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string[] languages = {"ru", "en"};
            if (languages.All(x => x != language.Language))
                return BadRequest(new {language = "Language dos not exist"});

            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var user = _userRepository.GetSingle(x => x.Id == userId);
            user.Language = language.Language;

            _userRepository.Update(user);
            _userRepository.Commit();
            return Ok();
        }
    }
}