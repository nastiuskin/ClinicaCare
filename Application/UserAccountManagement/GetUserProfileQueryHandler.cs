using Application.Auth;
using Application.Configuration.Queries;
using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Application.UserAccountManagement
{
    public record GetUserProfileQuery
    : IQuery<Result<UserViewDto>>;


    public class GetUserProfileQueryHandler
        : IQueryHandler<GetUserProfileQuery, Result<UserViewDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;


        public GetUserProfileQueryHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }

        public async Task<Result<UserViewDto>> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            var userIdResult = _jwtService.GetUserIdFromTokenAsync(_httpContextAccessor);
            if (userIdResult.IsFailed)
                return Result.Fail(userIdResult.Errors);

            var userId = new UserId(userIdResult.Value);

            var user = await _userRepository.GetByIdAsync(userId);

            var userViewDto = _mapper.Map<UserViewDto>(user);
            return Result.Ok(userViewDto);
        }
    }
}
