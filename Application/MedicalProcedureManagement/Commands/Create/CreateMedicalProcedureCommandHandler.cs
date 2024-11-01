using Application.Configuration.Commands;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;
using FluentValidation;

namespace Application.MedicalProcedureManagement.Commands.Create
{
    public record CreateMedicalProcedureCommand(MedicalProcedureFormDto MedicalProcedureCreateDto)
        : ICommand<Result>;

    public class CreateMedicalProcedureCommandHandler
        : ICommandHandler<CreateMedicalProcedureCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MedicalProcedureFormDto> _validator;

        public CreateMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository,
            IMapper mapper, IValidator<MedicalProcedureFormDto> validator, IUserRepository userRepository)
        {
            _mapper = mapper;
            _medicalProcedureRepository = medicalProcedureRepository;
            _validator = validator;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(CreateMedicalProcedureCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command.MedicalProcedureCreateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            if (await _medicalProcedureRepository.GetByNameAsync(command.MedicalProcedureCreateDto.Name) != null)
                return Result.Fail(ResponseError.Dublicated(nameof(command.MedicalProcedureCreateDto.Name)).Message);

            var medicalProcedure = _mapper.Map<MedicalProcedure>(command.MedicalProcedureCreateDto);

            if (command.MedicalProcedureCreateDto.Doctors?.Any() == true)
            {
                var doctorIds = command.MedicalProcedureCreateDto.Doctors.Select(id => new UserId(id)).ToList();
                var doctors = await _userRepository.GetListOfDoctorsByIdsAsync(doctorIds);

                doctors.ToList().ForEach(doctor => medicalProcedure.AssignDoctor(doctor));
            }
            await _medicalProcedureRepository.AddAsync(medicalProcedure);
            return Result.Ok();
        }
    }
}
