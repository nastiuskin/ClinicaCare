using Application.Configuration.Commands;
using Application.UserAccountManagement.Patients.UserDtos;
using AutoMapper;
using Domain.Users;
using Domain.Users.Patients;
using FluentResults;
using FluentValidation;

namespace Application.UserAccountManagement.Patients.Commands.Create
{
    public record PatientCreateCommand(UserFormDto PatientFormDto)
        : ICommand<Result>;

    public class PatientCreateCommandHandler
        : ICommandHandler<PatientCreateCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserFormDto> _validator;

        public PatientCreateCommandHandler(IUserRepository userRepository,
            IMapper mapper, IValidator<UserFormDto> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result> Handle(PatientCreateCommand command,
           CancellationToken cancellationToken)
        {
            var patientValidationResult = await _validator.ValidateAsync(command.PatientFormDto);

            if (!patientValidationResult.IsValid)
            {
                var errors = patientValidationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var existingPatient = await _userRepository.GetByEmailAsync(command.PatientFormDto.Email);
            if (existingPatient != null) return Result.Fail("Email in use");

            var patient = _mapper.Map<Patient>(command.PatientFormDto);
            await _userRepository.AddAsync(patient);
            return Result.Ok();
        }

    }
}
