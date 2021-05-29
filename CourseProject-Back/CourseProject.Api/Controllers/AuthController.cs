using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Auth;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;

        public AuthController(ITokenService tokenService, IUserRepository userRepository,
            IRolesRepository rolesRepository)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _rolesRepository = rolesRepository;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Post([FromBody] LoginViewModel loginModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userRepository.GetSingle(u => u.Email == loginModel.Email, u => u.Role);
            if (user == null) return BadRequest(new {error = "no user with this email"});
            var passwordValid = _tokenService.VerifyPassword(loginModel.Password, user.Password);
            if (!passwordValid) return BadRequest(new {error = "invalid password"});
            if (!user.IsActive) return BadRequest(new {error = "User is blocked and can't login"});

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Role, user.Role.Name)
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            user.LastLoginDate = DateTime.Now;

            _userRepository.Update(user);
            _userRepository.Commit();

            return Ok(new
            {
                user.Id,
                user.Username,
                role = user.Role.Name,
                user.Language,
                accessToken,
                refreshToken,
                user.DesignTheme
            });
        }

        [HttpPost("getLoginInUser")]
        public ActionResult GetLoginInUser()
        {
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers["Authorization"].ToString()
                    .Replace("Bearer ", ""));
            var user = _userRepository.GetSingle(u => u.Id == principal.Identity.Name, x => x.Role);
            if (user == null) return BadRequest("User not found");

            return Ok(new
                {
                    user.Id,
                    user.Username,
                    role = user.Role.Name,
                    user.Language,
                    user.DesignTheme,
                    refreshToken = user.RefreshToken,
                    accessToken = Request.Headers["Authorization"].ToString()
                        .Replace("Bearer ", "")
                }
            );
        }

        [HttpPost("register")]
        public ActionResult Post([FromBody] RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailUniq = _userRepository.IsEmailUniq(registerModel.Email);
            if (!emailUniq) return BadRequest(new {email = "user with this email already exists"});
            var usernameUniq = _userRepository.IsUsernameUniq(registerModel.Username);
            if (!usernameUniq) return BadRequest(new {username = "user with this email already exists"});

            var id = Guid.NewGuid().ToString();

            var user = new User
            {
                Id = id,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = _tokenService.HashPassword(registerModel.Password),
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Role = _rolesRepository.GetSingle(x => x.Name == "User")
            };

            _userRepository.Add(user);
            _userRepository.Commit();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new
            {
                user.Id,
                user.Username,
                role = user.Role.Name,
                user.Language,
                accessToken,
                refreshToken = user.RefreshToken,
                user.DesignTheme
            });
        }

        [HttpPost("google")]
        public async Task<ActionResult> GoogleLogIn([FromBody] GoogleLodInViewModel userView)
        {
            var payload = await GoogleJsonWebSignature
                .ValidateAsync(userView.TokenId, new GoogleJsonWebSignature.ValidationSettings());
            if (payload == null) return BadRequest("invalid token");
            var user = _userRepository.GetSingle(u => u.Email == payload.Email, x => x.Role);
            if (user == null) return BadRequest("no user with this email");
            if (!user.IsActive) return BadRequest("User is blocked and can't login");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            user.LastLoginDate = DateTime.Now;

            _userRepository.Update(user);
            _userRepository.Commit();

            return Ok(new
            {
                user.Id,
                user.Username,
                role = user.Role.Name,
                user.Language,
                accessToken,
                refreshToken,
                user.DesignTheme
            });
        }

        [HttpPost("facebook")]
        public async Task<ActionResult> FacebookLogIn([FromBody] FacebookLodInViewModel userView)
        {
            Console.WriteLine("User token " + userView.Token);
            var client = new HttpClient();
            var request = "https://graph.facebook.com/v10.0/me?fields=id%2Cname%2Cemail&access_token=" +
                          userView.Token;
            var res = await client.GetAsync(request);
            var content = res.Content;
            var data = content.ReadAsStringAsync().Result;
            var contact = JsonConvert.DeserializeObject<FacebookUserData>(data);
            if (contact != null && (contact.Email == null || contact.Id == null || contact.Name == null))
                return BadRequest("invalid token");

            var user = _userRepository.GetSingle(u => u.Email == contact.Email, x => x.Role);
            if (user == null) return BadRequest("no user with this email");
            if (!user.IsActive) return BadRequest("User is blocked and can't login");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            user.LastLoginDate = DateTime.Now;

            _userRepository.Update(user);
            _userRepository.Commit();

            return Ok(new
            {
                user.Id,
                user.Username,
                role = user.Role.Name,
                user.Language,
                accessToken,
                refreshToken,
                user.DesignTheme
            });
        }
    }
}