using Domain.Entities.Doctors;
using Domain.Entities.enums;
using Domain.Entities.MedicalServices;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Appointment
{
    public class Appointment
    {
        private readonly Doctor _doctor;
        private readonly Patient.Patient _patient;
        private readonly MedicalService _service;

        public int Id { get; private set; }

        [Required]
        public DateTime AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string Notes { get; private set; }

        public Appointment(Doctor doctor, Patient.Patient patient, MedicalService service, DateTime appointmentDateTime)
        {
            _doctor = doctor;
            _patient = patient;
            _service = service;

            AppointmentDateTime = appointmentDateTime;
            Status = AppointmentStatus.SCHEDULED;
            Notes = string.Empty;
        }

        public void AddNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                throw new ArgumentException("Notes cannot be empty", nameof(notes));

            Notes = notes;
        }

        public void Reschedule(DateTime newDateTime)
        {
            AppointmentDateTime = newDateTime;
        }
    }
}
