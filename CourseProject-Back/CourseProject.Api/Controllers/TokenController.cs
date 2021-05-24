using System;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Auth;
using CourseProject.Data.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userContext;

        public TokenController(IUserRepository userContext, ITokenService tokenService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("refresh")] 
        public ActionResult Refresh([FromBody]TokenApiViewModel tokenApiModel)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest("Model state is invalid"); 
                var accessToken = tokenApiModel.AccessToken;
                var refreshToken = tokenApiModel.RefreshToken;
                Console.WriteLine(tokenApiModel.AccessToken);
                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var userId = principal.Identity?.Name; //this is mapped to the Name claim by default

                var user = _userContext.GetSingle(u =>
                    refreshToken != null && u.RefreshToken == refreshToken && u.Id == userId);

                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now ||
                    !user.IsActive) return BadRequest("Invalid client request");

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                user.RefreshToken = _tokenService.GenerateRefreshToken();

                _userContext.Update(user);
                _userContext.Commit();

                return new ObjectResult(new
                {
                    accessToken = newAccessToken,
                    refreshToken = user.RefreshToken
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [Authorize]
        [HttpPost("revoke")]
        public IActionResult Revoke()
        {
            var userEmail = User.Identity?.Name;

            var user = _userContext.GetSingle(u => u.Email == userEmail);

            if (user == null) return BadRequest();

            if (user.RefreshToken == null) return Unauthorized();

            user.RefreshToken = null;

            _userContext.Update(user);
            _userContext.Commit();

            return NoContent();
        }
    }
}