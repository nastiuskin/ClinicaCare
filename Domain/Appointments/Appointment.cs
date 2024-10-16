using Domain.Doctors;
using Domain.MedicalServices;
using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace Domain.Appointments
{
    public class Appointment : IAgregateRoot
    {
        public int Id { get; private set; }
        private readonly Doctor _doctor;
        private readonly Patient.Patient _patient;
        private readonly MedicalProcedure _medicalProcedure;

        [Required]
        public TimeSlot AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        //create method vor validating appointment creation calling methods isAvailableTime etc
        public Appointment(Doctor doctor, Patient.Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
        {
            _doctor = doctor;
            _patient = patient;
            _medicalProcedure = medicalProcedure;

            AppointmentDateTime = appointmentDateTime;
            Status = AppointmentStatus.SCHEDULED;
            DoctorFeedback = string.Empty;
        }

        public void AddFeedback(string feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback))
                throw new ArgumentException("Notes cannot be empty", nameof(feedback));

            DoctorFeedback = feedback;
        }

        public void UpdateStatus(AppointmentStatus newStatus)
        {
            Status = newStatus;
        }
    }
}

