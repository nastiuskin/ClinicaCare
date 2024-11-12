using Application.Configuration.Commands;
using Application.UserAccountManagement.UserDtos;
using AutoMapper;
using Domain.Users;
using Domain.Users.Patients;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Register
{
    public record PatientRegisterCommand(UserFormDto PatientFormDto)
        : ICommand<Result>;

    public class PatientRegisterCommandHandler
        : ICommandHandler<PatientRegisterCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<UserFormDto> _validator;
        private readonly UserManager<User> _userManager;

        public PatientRegisterCommandHandler(IMapper mapper,
            IValidator<UserFormDto> validator,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<Result> Handle(PatientRegisterCommand command,
           CancellationToken cancellationToken)
        {
            var patientValidationResult = await _validator.ValidateAsync(command.PatientFormDto);
            if (!patientValidationResult.IsValid)
            {
                var errors = patientValidationResult.Errors
                    .Select(error => new Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var existingPatient = await _userManager.FindByEmailAsync(command.PatientFormDto.Email);
            if (existingPatient != null)
                return Result.Fail("Email is already in use");

            var patient = _mapper.Map<Patient>(command.PatientFormDto);

            var result = await _userManager.CreateAsync(patient, command.PatientFormDto.Password);
            if (!result.Succeeded)
            {
                var identityErrors = result.Errors
                    .Select(error => new Error(error.Description))
                    .ToList();
                return Result.Fail(identityErrors);
            }

            var roleResult = await _userManager.AddToRoleAsync(patient, "Patient");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(error => new Error(error.Description)).ToList();
                return Result.Fail(errors);
            }
            return Result.Ok();
        }

    }
}
