using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
                Expires = DateTime.Now.AddMinutes(5),
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
            if (!Guid.TryParse(cleanUserId, out var userIdGuid)) 
                return Result.Fail("Invalid user ID format");

            return Result.Ok(userIdGuid);
        }

        public Result ValidateRefreshTokenAsync(User user, string refreshToken)
        {
            if (!refreshToken.Equals(user.RefreshToken))
                return Result.Fail("Refresh token is not valid");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Result.Fail("Refresh token has expired. Please Log in");
            }

            return Result.Ok();
        }

        public Result<string> GetBearerToken(IHttpContextAccessor httpContextAccessor)
        {
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"];

            var accessToken = authorizationHeader.HasValue && !StringValues.IsNullOrEmpty(authorizationHeader.Value)
                ? authorizationHeader.Value.ToString().Replace("Bearer ", "")
                : string.Empty;

            if (string.IsNullOrEmpty(accessToken))
                return Result.Fail("Access token is missing.");
            return Result.Ok(accessToken);
         }

        public Result<string> GetEmailFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
                return Result.Fail("User email claim is missing or invalid.");
            return Result.Ok(emailClaim);
        }

        public Result<Guid> GetUserIdFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (string.IsNullOrEmpty(idClaim))
                return Result.Fail("User id claim is missing or invalid.");

            var cleanUserId = idClaim.Replace("UserId { Value = ", "").Replace(" }", "").Trim();
            if (!Guid.TryParse(cleanUserId, out var userIdGuid)) 
                return Result.Fail("Invalid user ID format");

            return Result.Ok(userIdGuid);
        }
    }
}