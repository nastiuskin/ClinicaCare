using Application.Configuration.Commands;
using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.UserAccountManagement
{
    public record UpdateUserCommand(UserViewDto UserUpdateDto)
        : ICommand<Result>;

    public class UpdateUserCommandHandler
        : ICommandHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserViewDto> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UserViewDto> validator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command.UserUpdateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                   .Select(error => new FluentResults.Error(error.ErrorMessage))
                   .ToList();
                return Result.Fail(errors);
            }
            var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepository.GetByEmailAsync(userEmail);

            _mapper.Map(command.UserUpdateDto, user);
            await _userRepository.UpdateAsync(user);

            return Result.Ok();
        }
    }

}
