using Application.Configuration.Commands;
using Application.SeedOfWork;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.Users.Doctors;
using FluentResults;

namespace Application.MedicalProcedureManagement.Commands.Update
{
    public record AddDoctorToMedicalProcedureCommand(Guid Id, Guid DoctorId) 
        : ICommand<Result>;

    public class AddDoctorToMedicalProcedureCommandHandler 
        : ICommandHandler<AddDoctorToMedicalProcedureCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        private readonly IUserRepository _userRepository;

        public AddDoctorToMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository, 
            IUserRepository userRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(AddDoctorToMedicalProcedureCommand command, 
            CancellationToken cancellation)
        {
            var medicalProcedure = await _medicalProcedureRepository
                .GetByIdWithDoctorsAsync(new MedicalProcedureId(command.Id));
            if (medicalProcedure == null) 
                return Result.Fail(ResponseError.NotFound(nameof(medicalProcedure), command.Id).Message);

            var doctor = await _userRepository.GetByIdAsync(new UserId(command.DoctorId));
            if (doctor == null)
                return Result.Fail(ResponseError.NotFound(nameof(doctor), command.DoctorId));

            if (doctor is not Doctor)
                return Result.Fail("User is not a doctor"); 

            var assignDoctorResult = medicalProcedure.AssignDoctor((Doctor)doctor);
            if (assignDoctorResult.IsFailed) 
                return Result.Fail(assignDoctorResult.Errors);

            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);
            return Result.Ok();
        }
    }
}
