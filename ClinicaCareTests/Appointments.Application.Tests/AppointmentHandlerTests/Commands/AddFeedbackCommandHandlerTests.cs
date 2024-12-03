using Application.AppointmentManagement.Commands.Complete;
using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;
using Moq;

namespace ClinicaCareTests.Appointments.Application.Tests.AppointmentHandlerTests.Commands
{
    public class AddFeedbackCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly AddFeedbackToAppointmentCommandHandler _addFeedbackCommandHandler;

        public AddFeedbackCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            _addFeedbackCommandHandler = new AddFeedbackToAppointmentCommandHandler(
                _appointmentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenAppointmentNotFound()
        {
            var command = new AddFeedbackToAppointmentCommand(Guid.NewGuid(), "Feedback");
            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
            .ReturnsAsync((Appointment)null);

            var result = await _addFeedbackCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("Appointment not found", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenFeedbackIsEmpty()
        {
            var command = new AddFeedbackToAppointmentCommand(Guid.NewGuid(), "");
            var appointment = CreateDefaultAppointment();

            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
                .ReturnsAsync(appointment);

            var result = await _addFeedbackCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("Feedback cannot be empty", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenStatusIsNotCompleted()
        {
            var command = new AddFeedbackToAppointmentCommand(Guid.NewGuid(), "Feedback");
            var appointment = CreateDefaultAppointment();

            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
                .ReturnsAsync(appointment);
            var result = await _addFeedbackCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("You can add feedback", result.Errors[0].Message);

        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAppointmentExistsFeedbackIsNotEmptyStatusIsCompleted()
        {
            var command = new AddFeedbackToAppointmentCommand(Guid.NewGuid(), "Feedback");
            var appointment = CreateDefaultAppointment();

            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
                .ReturnsAsync(appointment);

            appointment.Complete();
            var result = await _addFeedbackCommandHandler.Handle(command, default);

            Assert.True(result.IsSuccess);
            _appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Appointment>()), Times.Once());
        }

        public Appointment CreateDefaultAppointment()
        {
            var appointment = Appointment.Create
           (
              new UserId(Guid.NewGuid()),
              new UserId(Guid.NewGuid()),
              new MedicalProcedureId(Guid.NewGuid()),
              DateOnly.Parse("04.12.2024"),
              TimeSlot.Create(TimeSpan.FromMinutes(45), TimeSpan.FromMinutes(55)).Value
            );

            return appointment.Value;
        }
    }
}
