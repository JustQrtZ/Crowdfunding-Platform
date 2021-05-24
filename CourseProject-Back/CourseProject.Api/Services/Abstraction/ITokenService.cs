using System.Collections.Generic;
using System.Security.Claims;

namespace CourseProject.Api.Services.Abstraction
{
    public interface ITokenService
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public string GetUserIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
        public string GetRoleFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
    }
}