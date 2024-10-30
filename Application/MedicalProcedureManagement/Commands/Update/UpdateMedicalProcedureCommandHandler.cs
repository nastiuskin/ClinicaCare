using Application.Configuration.Commands;
using Application.MedicalProcedureManagement.DTO;
using Application.SeedOfWork;
using AutoMapper;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.Users.Doctors;
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

            var doctorsToAdd = command.MedicalProcedureUpdateDto.DoctorsToAdd;
            if (doctorsToAdd != null && doctorsToAdd.Count > 0)
            {
                foreach (var doctorId in doctorsToAdd)
                {
                    var doctor = await _userRepository.GetByIdAsync(new UserId(doctorId));
                    if (doctor == null || doctor is not Doctor) continue;

                    var assignDoctorResult = medicalProcedure.AssignDoctor((Doctor)doctor);
                    if (assignDoctorResult.IsFailed) continue;
                }
            }

            var doctorsToRemove = command.MedicalProcedureUpdateDto.DoctorsToRemove;
            if (doctorsToRemove != null && doctorsToRemove.Count > 0)
            {
                foreach (var doctorId in doctorsToRemove)
                {
                    var doctor = await _userRepository.GetByIdAsync(new UserId(doctorId));
                    if (doctor == null || doctor is not Doctor) continue;

                    var assignDoctorResult = medicalProcedure.RemoveDoctor((Doctor)doctor);
                    if (assignDoctorResult.IsFailed) continue;
                }
            }
            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);

            var medicalProcedureDto = _mapper.Map<MedicalProcedureInfoWithDoctorsDto>(medicalProcedure);

            return Result.Ok(medicalProcedureDto);
        }

    }
}
