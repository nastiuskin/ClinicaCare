using Application.Configuration.Commands;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Commands.Update
{
    public record MedicalProcedureChangeDurationCommand(Guid Id, TimeSpan Duration) : ICommand<Result>;


    public class MedicalProcedureChangeDurationCommandHandler : ICommandHandler<MedicalProcedureChangeDurationCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;
        public MedicalProcedureChangeDurationCommandHandler(IMedicalProcedureRepository medicalProcedureRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
        }

        //to create MedicalProcedureError????? NotFound,InvalidDuration, InvalidPrice ???
        public async Task<Result> Handle(MedicalProcedureChangeDurationCommand command, CancellationToken cancellationToken)
        {
            var medicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(command.Id));
            if (medicalProcedure == null) return Result.Fail("Medical procedure not found"); // )))))) 

            var updateResult = medicalProcedure.UpdateDuration(command.Duration);
            if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);
            return Result.Ok();
        }

    }
}
