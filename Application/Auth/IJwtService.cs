﻿using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Application.Auth
{
    public interface IJwtService
    {
        public Task<Result<TokenDto>> GenerateTokensAsync(User user);
        public Task<Result> RevokeRefreshTokenAsync(string email);
        public Result<Guid> GetUserIdFromTokenAsync(IHttpContextAccessor httpContextAccessor);
        public Result ValidateRefreshTokenAsync(User user,string refreshToken);
    }
}
