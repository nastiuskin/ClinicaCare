using Domain.Entities.Appointment;
using Domain.Entities.Appointments;
using Domain.Entities.Doctors;
using Domain.Entities.enums;
using Domain.Entities.MedicalServices;
using Domain.Entities.Patient;

namespace ClinicaCareTests
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public void GetAvailableTimeSlots_Returns_Correct_TimeSlots()
        {
            // Arrange
            var doctor = new Doctor("John", "Doe", "john.doe@example.com", "+123456789", SpecializationType.GENERALPRACTITIONER, "Bio", 101);
            var schedule = new Schedule(doctor, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)); // 9 AM to 5 PM
            var service = new MedicalService(ServiceType.CONSULTATION, 50.0m, new TimeSpan(1,0, 0)); // 1 hours consultation

            // Act
            var availableSlots = schedule.GetAvailableTimeSlots(service);

            // Assert
            Assert.IsNotNull(availableSlots);
            Assert.AreEqual(8, availableSlots.Count); // 8 hours * 2 slots per hour = 16 slots
            Assert.AreEqual(availableSlots[0].startTime.TimeOfDay, new TimeSpan(9, 0, 0)); // First slot starts at 9:00 AM
            Assert.AreEqual(availableSlots[0].endTime.TimeOfDay, new TimeSpan(10, 0, 0)); // First slot ends at 9:30 AM
            Assert.AreEqual(availableSlots.Last().startTime.TimeOfDay, new TimeSpan(16, 0, 0)); // Last slot starts at 4:30 PM
            Assert.AreEqual(availableSlots.Last().endTime.TimeOfDay, new TimeSpan(17, 0, 0)); // Last slot ends at 5:00 PM
        }

    }
}