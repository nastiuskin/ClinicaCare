using Domain.Entities.Appointment;
using Domain.Entities.Appointments;
using Domain.Entities.Doctors;
using Domain.Entities.enums;
using Domain.Entities.MedicalServices;
using Domain.Entities.Patient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ScheduleTests
    {
        private Doctor _doctor;
        private MedicalService _service;
        private Schedule _schedule;

        [SetUp]
        public void SetUp()
        {
            _doctor = new Doctor("John", "Doe", "john.doe@example.com", "+1234567890",
                     SpecializationType.CARDIOLOGIST, "Biography", 101);
            _service = new MedicalService(ServiceType.CONSULTATION, 50.0m, TimeSpan.FromMinutes(30));
            _schedule = new Schedule(_doctor, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)); // 9 AM to 5 PM
        }

        [Test]
        public void GetAvailableTimeSlots_ReturnsValidSlots_ExcludingExistingAppointments()
        {
            // Arrange
            var existingAppointments = new List<Appointment>
            {
                new Appointment(_doctor, new Patient("John", "Doe", "john.doe@example.com", "+1234567890", DateTime.Today.AddYears(-25)),
                                _service, new TimeSlot(DateTime.Today.AddHours(9), DateTime.Today.AddHours(9.5))), // 09:00 - 09:30
                new Appointment(_doctor, new Patient("Jane", "Doe", "jane.doe@example.com", "+0987654321", DateTime.Today.AddYears(-30)),
                                _service, new TimeSlot(DateTime.Today.AddHours(10), DateTime.Today.AddHours(10.5))) // 10:00 - 10:30
            };

            // Add appointments to the schedule
            foreach (var appointment in existingAppointments)
            {
                _schedule.AddAppointment(appointment);
            }

            // Act
            var availableSlots = _schedule.GetAvailableTimeSlots(_service);

            // Assert
            Assert.AreEqual(8, availableSlots.Count); // Expecting 8 available slots (09:30 to 17:00)
            Assert.IsFalse(availableSlots.Any(slot => slot.StartTime < existingAppointments[0].AppointmentDateTime.StartTime));
            Assert.IsFalse(availableSlots.Any(slot => slot.StartTime < existingAppointments[1].AppointmentDateTime.StartTime));
        }

        [Test]
        public void GetAvailableTimeSlots_ReturnsAllSlots_WhenNoAppointmentsExist()
        {
            // Act
            var availableSlots = _schedule.GetAvailableTimeSlots(_service);

            // Assert
            Assert.AreEqual(16, availableSlots.Count); // Expecting 16 slots from 9 AM to 5 PM (30 minutes each)
        }

        [Test]
        public void AddAppointment_ThrowsException_WhenTimeSlotIsNotAvailable()
        {
            // Arrange
            var appointment = new Appointment(_doctor, new Patient("Alice", "Smith", "alice.smith@example.com", "+1234567890", DateTime.Today.AddYears(-25)),
                                              _service, new TimeSlot(DateTime.Today.AddHours(9), DateTime.Today.AddHours(9.5))); // 09:00 - 09:30

            // Act
            _schedule.AddAppointment(appointment);

            // Try to add an overlapping appointment
            var overlappingAppointment = new Appointment(_doctor, new Patient("Bob", "Brown", "bob.brown@example.com", "+0987654321", DateTime.Today.AddYears(-30)),
                                                         _service, new TimeSlot(DateTime.Today.AddHours(9), DateTime.Today.AddHours(10))); // 09:00 - 10:00

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _schedule.AddAppointment(overlappingAppointment));
            Assert.AreEqual("The selected time slot is not available.", ex.Message);
        }

        [Test]
        public void IsTimeAvailable_ReturnsFalse_WhenTimeSlotIsOutsideWorkingHours()
        {
            // Arrange
            var timeSlot = new TimeSlot(DateTime.Today.AddHours(8), DateTime.Today.AddHours(9)); // 08:00 - 09:00

            // Act
            var isAvailable = _schedule.IsTimeAvailable(timeSlot);

            // Assert
            Assert.IsFalse(isAvailable);
        }
    }
}
