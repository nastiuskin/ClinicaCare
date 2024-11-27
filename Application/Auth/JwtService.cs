using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Auth
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;

        public JwtService(IConfiguration configuration, UserManager<User> userManager)
        {
            _config = configuration;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ApplicationSettings:JWT:SigningKey"]));
        }

        public async Task<Result<TokenDto>> GenerateTokensAsync(User user)
        {
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Email, user.Email),
              new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddSeconds(30),
                SigningCredentials = creds,
                Issuer = _config["ApplicationSettings:JWT:Issuer"],
                Audience = _config["ApplicationSettings:JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            var setRefreshTokenResult = user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            if (!setRefreshTokenResult.IsSuccess)
                return Result.Fail(setRefreshTokenResult.Errors);

            await _userManager.UpdateAsync(user);

            return Result.Ok(new TokenDto(accessToken, refreshToken));
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<Result> RevokeRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) 
                return Result.Fail("User not found");

            var deleteRefreshTokenResult = user.DeleteRefreshToken();
            if (!deleteRefreshTokenResult.IsSuccess) 
                return Result.Fail(deleteRefreshTokenResult.Errors);

            await _userManager.UpdateAsync(user);

            return Result.Ok();
        }

        public Result<Guid> GetUserIdFromTokenAsync(IHttpContextAccessor httpContextAccessor)
        {
            var patientIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientIdClaim))
                return Result.Fail("User id claim is missing or invalid.");

            var cleanUserId = patientIdClaim.Replace("UserId { Value = ", "").Replace(" }", "").Trim();
            if (!Guid.TryParse(cleanUserId, out var userIdGuid)) return Result.Fail("Invalid user ID format");
            return Result.Ok(userIdGuid);
        }

        public Result ValidateRefreshTokenAsync(User user, string refreshToken)
        {
            if (!refreshToken.Equals(user.RefreshToken)) 
                return Result.Fail("Refresh token is not valid");

            return Result.Ok();
        }
    }
}