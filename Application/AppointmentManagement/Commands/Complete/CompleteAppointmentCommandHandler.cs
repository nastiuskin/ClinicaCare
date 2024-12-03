using Application.Configuration.Queries;
using Application.SeedOfWork;
using Domain.Appointments;
using FluentResults;

namespace Application.AppointmentManagement.Commands.Complete
{
    public record CompleteAppointmentCommand(Guid Id)
        : IQuery<Result>;

    public class CompleteAppointmentCommandHandler
        : IQueryHandler<CompleteAppointmentCommand, Result>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result> Handle(CompleteAppointmentCommand command,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(new AppointmentId(command.Id));
            if (appointment == null) return Result.Fail(new FluentResults.Error("Appointment not found"));

            var result = appointment.Complete();
            if (!result.IsSuccess) 
                return Result.Fail(result.Errors);

            await _appointmentRepository.UpdateAsync(appointment);
            return Result.Ok();
        }
    }
}
