using Application.Configuration.Commands;
using Application.SeedOfWork;
using Domain.Appointments;
using FluentResults;

namespace Application.AppointmentManagement.Commands.Complete
{
    public record AddFeedbackToAppointmentCommand(Guid Id, string FeedBack)
        : ICommand<Result>;

    public class AddFeedbackToAppointmentCommandHandler
        : ICommandHandler<AddFeedbackToAppointmentCommand, Result>
    {
        private IAppointmentRepository _appointmentRepository;
        public AddFeedbackToAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result> Handle(AddFeedbackToAppointmentCommand command,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(new AppointmentId(command.Id));
            if (appointment == null) return Result.Fail(new FluentResults.Error("Appointment not found"));

            var feedbackResult = appointment.AddFeedback(command.FeedBack);
            if (feedbackResult.IsFailed)
                return Result.Fail(feedbackResult.Errors);

            await _appointmentRepository.UpdateAsync(appointment);
            return Result.Ok();
        }
    }
}
