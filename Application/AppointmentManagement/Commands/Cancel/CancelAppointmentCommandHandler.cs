using Application.Configuration.Queries;
using Application.SeedOfWork;
using Domain.Appointments;
using FluentResults;

namespace Application.AppointmentManagement.Commands.Cancel
{
    public record CancelAppointmentCommand(Guid Id)
        : IQuery<Result>;


    public class CancelAppointmentCommandHandler
        : IQueryHandler<CancelAppointmentCommand, Result>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result> Handle(CancelAppointmentCommand command,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(new AppointmentId(command.Id));
            if (appointment == null) return Result.Fail(new FluentResults.Error("Appointment not found"));

            var result = appointment.Cancel();
            if (!result.IsSuccess)
                return Result.Fail(result.Errors);

            await _appointmentRepository.UpdateAsync(appointment);
            return Result.Ok();
        }
    }
}


