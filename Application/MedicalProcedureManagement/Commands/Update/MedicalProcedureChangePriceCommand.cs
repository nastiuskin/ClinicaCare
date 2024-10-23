using Application.Configuration.Commands;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Commands.Update
{
    public record MedicalProcedureChangePriceCommand(Guid Id, decimal Price) : ICommand<Result>;


    public class MedicalProcedureChangePriceCommandHandler : ICommandHandler<MedicalProcedureChangePriceCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;

        public MedicalProcedureChangePriceCommandHandler(IMedicalProcedureRepository medicalProcedureRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
        }

        public async Task<Result> Handle(MedicalProcedureChangePriceCommand command, CancellationToken cancellation)
        {
            var medicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(command.Id));
            if (medicalProcedure == null) return Result.Fail("Medical Procedure not found"); // )))))))

            var updateResult = medicalProcedure.UpdatePrice(command.Price);
            if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

            await _medicalProcedureRepository.UpdateAsync(medicalProcedure);
            return Result.Ok();
        }
    }
}
