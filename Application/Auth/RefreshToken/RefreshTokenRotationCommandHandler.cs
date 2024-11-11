using Application.Configuration.Commands;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Auth.RefreshToken
{
    public record RefreshTokenRotationCommand()
        : ICommand<Result<string>>;

    public class RefreshTokenRotationCommandHandler
        : ICommandHandler<RefreshTokenRotationCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenRotationCommandHandler(UserManager<User> userManager, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager=userManager;
            _jwtService=jwtService;
            _httpContextAccessor=httpContextAccessor;
        }

        public async Task<Result<string>> Handle(RefreshTokenRotationCommand command, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Result.Fail("Refresh token is missing.");

            var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
                return Result.Fail("User email claim is missing or invalid.");

            var user = await _userManager.FindByEmailAsync(emailClaim);
            if (user == null)
                return Result.Fail("User not found.");

            var validateRefreshTokenResult = _jwtService.ValidateRefreshTokenAsync(user, refreshToken);
            if (!validateRefreshTokenResult.IsSuccess)
                return Result.Fail(validateRefreshTokenResult.Errors);

            var tokenResult = await _jwtService.GenerateTokensAsync(user);
            if (!tokenResult.IsSuccess)
                return Result.Fail(tokenResult.Errors);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", tokenResult.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Result.Ok(tokenResult.Value.AccessToken);
        }
    }
}
