using Application.Configuration.Commands;
using Application.UserAccountManagement.UserDtos;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Login
{
    public record UserLoginCommand(UserLoginDto UserLoginDto)
        : ICommand<Result<string>>;

    public class UserLoginCommandHandler
        : ICommandHandler<UserLoginCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLoginCommandHandler(
            UserManager<User> userManager, 
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.UserLoginDto.Email);
            if (user == null)
                return Result.Fail("User not found.");

            var signInResult = await _userManager.CheckPasswordAsync(user, command.UserLoginDto.Password);
            if (!signInResult)
                return Result.Fail("Invalid email or password.");

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
