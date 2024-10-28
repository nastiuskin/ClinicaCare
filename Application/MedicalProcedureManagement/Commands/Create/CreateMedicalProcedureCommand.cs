using Application.Configuration.Commands;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;
using FluentValidation;

namespace Application.MedicalProcedureManagement.Commands.Create
{
    public record CreateMedicalProcedureCommand(MedicalProcedureFormDto MedicalProcedureCreateDto) : ICommand<Result<MedicalProcedure>>;

    public class CreateMedicalProcedureCommandHandler : ICommandHandler<CreateMedicalProcedureCommand, Result<MedicalProcedure>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MedicalProcedureFormDto> _validator;

        public CreateMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository, IMapper mapper, IValidator<MedicalProcedureFormDto> validator)
        {
            _mapper = mapper;
            _medicalProcedureRepository = medicalProcedureRepository;
            _validator = validator;
        }

        public async Task<Result<MedicalProcedure>> Handle(CreateMedicalProcedureCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command.MedicalProcedureCreateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var existingName = await _medicalProcedureRepository.GetByNameAsync(command.MedicalProcedureCreateDto.Name);
            if (existingName != null) return Result.Fail(RequestError.Dublicate().Message);

            var medicalProcedure = _mapper.Map<MedicalProcedure>(command.MedicalProcedureCreateDto);

            await _medicalProcedureRepository.AddAsync(medicalProcedure);
            return Result.Ok(medicalProcedure);
        }
    }
}
