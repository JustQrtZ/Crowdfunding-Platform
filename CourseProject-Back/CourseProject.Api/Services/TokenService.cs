using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CourseProject.Api.Services.Abstraction;
using CryptoHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWTSecretKey")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWTSecretKey"))),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        
        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }

        public string GetRoleFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
        }
        public string GetUserIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();;
        }
    }
}