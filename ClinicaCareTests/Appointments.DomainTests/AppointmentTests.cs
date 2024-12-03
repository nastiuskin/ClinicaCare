using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Domain.ValueObjects;

namespace ClinicaCareTests.Appointments.DomainTests
{
    public class AppointmentTests
    {
        [Fact]
        public void AppointmentCreate_ShouldReturnSuccessfulResult_WithValidParameters()
        {
            //Arrange

            var doctorId = new UserId(Guid.NewGuid());
            var patientId = new UserId(Guid.NewGuid());
            var medicalProcedureId = new MedicalProcedureId(Guid.NewGuid());
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var duration = TimeSlot.Create(TimeSpan.Parse("09:00"), TimeSpan.Parse("09:45")).Value;


            //Act
            var result = Appointment.Create(doctorId, patientId, medicalProcedureId, date, duration);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(doctorId, result.Value.DoctorId);
            Assert.Equal(patientId, result.Value.PatientId);
            Assert.Equal(medicalProcedureId, result.Value.MedicalProcedureId);
            Assert.Equal(date, result.Value.Date);
            Assert.Equal(duration, result.Value.Duration);
            Assert.Equal(AppointmentStatus.SCHEDULED, result.Value.Status);
        }



        [Fact]
        public void AddFeedback_Should_Fail_WhenFeedbackIsEmpty()
        {
            var appointment = CreateDefaultAppointment();
            var result = appointment.AddFeedback("");


            Assert.False(result.IsSuccess);
            Assert.Contains("Feedback cannot be empty", result.Errors[0].Message);
        }


        [Fact]
        public void AddFeedback_ShouldFail_WhenStatusIsNotCompleted()
        {
            var appointment = CreateDefaultAppointment();
            var result = appointment.AddFeedback("Feedback");

            Assert.False(result.IsSuccess);
            Assert.Contains("You can add feedback only if appointment status is completed", result.Errors[0].Message);
        }


        [Fact]
        public void AddFeedback_ShouldSucceed_WhenValidFeedbackProvidedAndStatusCompleted()
        {
            var appointment = CreateDefaultAppointment();
            appointment.Complete();

            var result = appointment.AddFeedback("Feedback");

            Assert.True(result.IsSuccess);
            Assert.Equal("Feedback", appointment.DoctorFeedback);
        }


        [Fact]
        public void Complete_ShouldChangeStatusToCompleted()
        {
            var appointment = CreateDefaultAppointment();
            var result = appointment.Complete();

            Assert.True(result.IsSuccess);
            Assert.Equal(AppointmentStatus.COMPLETED, appointment.Status);
        }

        [Fact]
        public void Cancel_ShouldChangeStatusToCanceled()
        {
            var appointment = CreateDefaultAppointment();
            var result = appointment.Cancel();

            Assert.True(result.IsSuccess);
            Assert.Equal(AppointmentStatus.CANCELED, appointment.Status);
        }


        private Appointment CreateDefaultAppointment()
        {
            var doctorId = new UserId(Guid.NewGuid());
            var patientId = new UserId(Guid.NewGuid());
            var medicalProcedureId = new MedicalProcedureId(Guid.NewGuid());
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var duration = TimeSlot.Create(TimeSpan.FromHours(1), TimeSpan.FromHours(2)).Value;
            return Appointment.Create(doctorId, patientId, medicalProcedureId, date, duration).Value;
        }
    }
}
