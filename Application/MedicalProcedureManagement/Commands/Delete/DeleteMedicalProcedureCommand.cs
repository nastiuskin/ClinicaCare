using Application.Configuration.Commands;
using Application.SeedOfWork;
using Domain.MedicalProcedures;
using FluentResults;

namespace Application.MedicalProcedureManagement.Commands.Delete
{
    public record DeleteMedicalProcedureCommand(Guid id) : ICommand<Result>;

    public class DeleteMedicalProcedureCommandHandler : ICommandHandler<DeleteMedicalProcedureCommand, Result>
    {
        private readonly IMedicalProcedureRepository _medicalProcedureRepository;

        public DeleteMedicalProcedureCommandHandler(IMedicalProcedureRepository medicalProcedureRepository)
        {
            _medicalProcedureRepository = medicalProcedureRepository;
        }

        public async Task<Result> Handle(DeleteMedicalProcedureCommand command, CancellationToken cancellationToken)
        {
            var medicalProcedure = await _medicalProcedureRepository.GetByIdAsync(new MedicalProcedureId(command.id));
            if (medicalProcedure == null) return Result.Fail(RequestError.NotFound(command.id).Message);

            await _medicalProcedureRepository.DeleteAsync(medicalProcedure);
            return Result.Ok(); ;
        }
    }

}
