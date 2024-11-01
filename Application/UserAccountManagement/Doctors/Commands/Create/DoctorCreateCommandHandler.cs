using Application.Configuration.Commands;
using Application.UserAccountManagement.Doctors.DTO;
using AutoMapper;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;
using FluentValidation;

namespace Application.UserAccountManagement.Doctors.Commands.Create
{
    public record DoctorCreateCommand(DoctorFormDto DoctorFormDto)
        : ICommand<Result>;


    public class DoctorCreateCommandHandler 
        : ICommandHandler<DoctorCreateCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<DoctorFormDto> _validator;

        public DoctorCreateCommandHandler(IUserRepository userRepository, 
            IMapper mapper, IValidator<DoctorFormDto> validator)
        {
            _userRepository = userRepository;
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

            var existingDoctor = await _userRepository.GetByEmailAsync(command.DoctorFormDto.Email);
            if (existingDoctor != null) return Result.Fail("Email in use");

            var doctor = _mapper.Map<Doctor>(command.DoctorFormDto);
       
            await _userRepository.AddAsync(doctor);
            return Result.Ok();
        }
    }

}
