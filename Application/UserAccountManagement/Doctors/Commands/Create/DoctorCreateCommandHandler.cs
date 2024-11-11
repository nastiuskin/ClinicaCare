using Application.Configuration.Commands;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.UserAccountManagement.Doctors.Commands.Create
{
    public record DoctorCreateCommand(DoctorFormDto DoctorFormDto)
        : ICommand<Result>;


    public class DoctorCreateCommandHandler
        : ICommandHandler<DoctorCreateCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<DoctorFormDto> _validator;

        public DoctorCreateCommandHandler(UserManager<User> userManager,
            IMapper mapper, IValidator<DoctorFormDto> validator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result> Handle(DoctorCreateCommand command,
            CancellationToken cancellationToken)
        {
            var doctorValidationResult = await _validator.ValidateAsync(command.DoctorFormDto);

            if (!doctorValidationResult.IsValid)
            {
                var errors = doctorValidationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var existingDoctor = await _userManager.FindByEmailAsync(command.DoctorFormDto.Email);
            if (existingDoctor != null) 
                return Result.Fail("Email in use");

            var doctor = _mapper.Map<Doctor>(command.DoctorFormDto);

            var result = await _userManager.CreateAsync(doctor, command.DoctorFormDto.Password);
            if (!result.Succeeded)
            {
                var identityErrors = result.Errors
                    .Select(error => new FluentResults.Error(error.Description))
                    .ToList();
                return Result.Fail(identityErrors);
            }

            var roleResult = await _userManager.AddToRoleAsync(doctor, "Doctor");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(error => new FluentResults.Error(error.Description)).ToList();
                return Result.Fail(errors);
            }
            return Result.Ok();
        }
    }
}
