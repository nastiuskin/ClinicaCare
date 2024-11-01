using Application.Configuration.Commands;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using FluentResults;
using FluentValidation;

namespace Application.MedicalProcedureManagement.Commands.Update
{
    public record UpdateMedicalProcedureCommand(Guid Id, MedicalProcedureUpdateDto MedicalProcedureUpdateDto)
        : ICommand<Result<MedicalProcedureInfoWithDoctorsDto>>;

    public class UpdateMedicalProcedureCommandHandler
        : ICommandHandler<UpdateMedicalProcedureCommand, Result<MedicalProcedureInfoWithDoctorsDto>>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MedicalProcedureUpdateDto> _validator;

        public UpdateMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository,
            IMapper mapper, IValidator<MedicalProcedureUpdateDto> validator, IUserRepository userRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _mapper = mapper;
            _validator = validator;
            _userRepository = userRepository;
        }

        public async Task<Result<MedicalProcedureInfoWithDoctorsDto>> Handle(UpdateMedicalProcedureCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command.MedicalProcedureUpdateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var medicalProcedure = await _medicalProcedureRepository
                .GetByIdWithDoctorsAsync(new MedicalProcedureId(command.Id));

            if (medicalProcedure == null)
                return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), command.Id).Message);

            _mapper.Map(command.MedicalProcedureUpdateDto, medicalProcedure);

            if (command.MedicalProcedureUpdateDto.DoctorsToAdd?.Any() == true)
            {
                var doctorsToAdd = await _userRepository.GetListOfDoctorsByIdsAsync(
                    command.MedicalProcedureUpdateDto.DoctorsToAdd.Select(id => new UserId(id)).ToList());

                doctorsToAdd.ToList().ForEach(doctor => medicalProcedure.AssignDoctor(doctor));
            }

            if (command.MedicalProcedureUpdateDto.DoctorsToRemove?.Any() == true)
            {
                var doctorsToRemove = await _userRepository.GetListOfDoctorsByIdsAsync(
                    command.MedicalProcedureUpdateDto.DoctorsToRemove.Select(id => new UserId(id)).ToList());

                doctorsToRemove.ToList().ForEach(doctor => medicalProcedure.RemoveDoctor(doctor));
            }
            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);

            var medicalProcedureDto = _mapper.Map<MedicalProcedureInfoWithDoctorsDto>(medicalProcedure);

            return Result.Ok(medicalProcedureDto);
        }

    }
}
