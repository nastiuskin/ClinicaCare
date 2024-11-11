using Application.Configuration.Commands;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Auth.Logout
{
    public record UserLogoutCommand
        : ICommand<Result>;

    public class UserLogoutCommandHandler
        : ICommandHandler<UserLogoutCommand, Result>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLogoutCommandHandler(SignInManager<User> signInManager, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(UserLogoutCommand command, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Result.Fail("User email not found.");

            var revokeResult = await _jwtService.RevokeRefreshTokenAsync(email);
            if (revokeResult.IsFailed)
                return Result.Fail(revokeResult.Errors);

            await _signInManager.SignOutAsync();
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

            return Result.Ok();
        }
    }
}
