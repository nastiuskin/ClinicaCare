using Application.Configuration.Commands;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Application.Auth.RefreshToken
{
    public record RefreshTokenRotationCommand()
        : ICommand<Result<string>>;

    public class RefreshTokenRotationCommandHandler
        : ICommandHandler<RefreshTokenRotationCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenRotationCommandHandler(IUserRepository userRepository, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository=userRepository;
            _jwtService=jwtService;
            _httpContextAccessor=httpContextAccessor;
        }

        public async Task<Result<string>> Handle(RefreshTokenRotationCommand command, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Result.Fail("Refresh token is missing.");

            var accessTokenResult = _jwtService.GetBearerToken(_httpContextAccessor);
            if (accessTokenResult.IsFailed)
                return Result.Fail(accessTokenResult.Errors);

            var idResult = _jwtService.GetUserIdFromToken(accessTokenResult.Value);
            if (idResult.IsFailed)
                return Result.Fail(idResult.Errors);


            var user = await _userRepository.GetByIdAsync(new UserId(idResult.Value));
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
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromDays(30)
            });

            return Result.Ok(tokenResult.Value.AccessToken);
        }
    }
}
