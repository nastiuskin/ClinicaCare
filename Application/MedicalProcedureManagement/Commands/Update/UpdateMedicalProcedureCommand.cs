using Application.Configuration.Commands;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using FluentResults;
using FluentValidation;

namespace Application.MedicalProcedureManagement.Commands.Update
{
    public record UpdateMedicalProcedureCommand(Guid Id, MedicalProcedureFormDto MedicalProcedureCreateDto) : ICommand<Result<MedicalProcedure>>;

    public class UpdateMedicalProcedureCommandHandler : ICommandHandler<UpdateMedicalProcedureCommand, Result<MedicalProcedure>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MedicalProcedureFormDto> _validator;

        public UpdateMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository, IMapper mapper, IValidator<MedicalProcedureFormDto> validator)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<MedicalProcedure>> Handle(UpdateMedicalProcedureCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command.MedicalProcedureCreateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var existingMedicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(command.Id));
            if (existingMedicalProcedure == null) return Result.Fail(RequestError.NotFound(command.Id).Message);

            _mapper.Map(command.MedicalProcedureCreateDto, existingMedicalProcedure);
            await _medicalProcedureRepository.UpdateAsync(existingMedicalProcedure);

            return Result.Ok(existingMedicalProcedure);
        }

    }
}
