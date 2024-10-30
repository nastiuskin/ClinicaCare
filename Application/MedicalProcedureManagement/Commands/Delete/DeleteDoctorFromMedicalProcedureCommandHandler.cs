using Application.Configuration.Commands;
using Application.SeedOfWork;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;

namespace Application.MedicalProcedureManagement.Commands.Delete
{
    public record DeleteDoctorFromMedicalProcedureCommand(Guid MedicalProcedureId, Guid DoctorId)
        : ICommand<Result>;


    public class DeleteDoctorFromMedicalProcedureCommandHandler 
        : ICommandHandler<DeleteDoctorFromMedicalProcedureCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;

        public DeleteDoctorFromMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository,
            IUserRepository userRepository)
        {
            _medicalProcedureRepository=medicalProcedureRepository;
            _userRepository=userRepository;
        }

        public async Task<Result> Handle(DeleteDoctorFromMedicalProcedureCommand command,
            CancellationToken cancellation)
        {
            var medicalProcedure = await _medicalProcedureRepository
                .GetByIdAsync(new MedicalProcedureId(command.MedicalProcedureId)); 

            if (medicalProcedure == null)
                return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), command.MedicalProcedureId).Message); 

            var doctor = await _userRepository.GetByIdAsync(new UserId(command.DoctorId));
            if (doctor == null)
                return Result.Fail(ResponseError.NotFound(nameof(doctor), command.DoctorId).Message); 

            if (doctor is not Doctor)
                return Result.Fail("User is not doctor");

            var deleteDoctorResult = medicalProcedure.RemoveDoctor((Doctor)doctor);

            if (deleteDoctorResult.IsFailed) 
                return Result.Fail(deleteDoctorResult.Errors);

            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);

            return Result.Ok();
        }
    }
}
