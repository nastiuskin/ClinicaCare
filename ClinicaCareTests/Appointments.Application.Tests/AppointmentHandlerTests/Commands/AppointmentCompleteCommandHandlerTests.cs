using Application.AppointmentManagement.Commands.Complete;
using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;
using Moq;

namespace ClinicaCareTests.Appointments.Application.Tests.AppointmentHandlerTests.Commands
{
    public class AppointmentCompleteCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly CompleteAppointmentCommandHandler _appointmentCompleteCommandHandler;

        public AppointmentCompleteCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _appointmentCompleteCommandHandler = new CompleteAppointmentCommandHandler(
                _appointmentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenAppointmentNotFound()
        {
            var command = new CompleteAppointmentCommand(Guid.NewGuid());
            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
            .ReturnsAsync((Appointment)null);

            var result = await _appointmentCompleteCommandHandler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Contains("Appointment not found", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAppointmentCompletedSuccessfully()
        {
            var command = new CompleteAppointmentCommand(Guid.NewGuid());

            var appointment = Appointment.Create
            (
               new UserId(Guid.NewGuid()),
               new UserId(Guid.NewGuid()),
               new MedicalProcedureId(Guid.NewGuid()),
               DateOnly.Parse("04.12.2024"),
               TimeSlot.Create(TimeSpan.FromMinutes(45), TimeSpan.FromMinutes(55)).Value
             );

            _appointmentRepositoryMock.Setup(ap => ap.GetByIdAsync(new AppointmentId(command.Id)))
                .ReturnsAsync(appointment.Value);

            var result = await _appointmentCompleteCommandHandler.Handle(command, default);

            Assert.True(result.IsSuccess);
            _appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Appointment>()), Times.Once());
        }
    }
}
