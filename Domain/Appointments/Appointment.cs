using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Domain.SeedWork.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Appointments
{
    public class Appointment : IAgregateRoot
    {
        public int Id { get; private set; }
        private readonly Doctor _doctor;
        private readonly Patient _patient;
        private readonly MedicalProcedure _medicalProcedure;

        [Required]
        public TimeSlot AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        private Appointment(Doctor doctor, Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
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

        public static Appointment Create(Doctor doctor, Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
        {
            if(doctor == null) throw new ArgumentNullException("Doctor cannot be null", nameof (doctor));
            if(patient == null) throw new ArgumentNullException("Patient cannot be null", nameof(_patient));
            if(medicalProcedure == null) throw new ArgumentNullException("Medical Procedure cannot be null", nameof (_medicalProcedure));
            if(appointmentDateTime == null) throw new ArgumentNullException("Appointment time is required", nameof(appointmentDateTime));
            return new Appointment(doctor, patient, medicalProcedure, appointmentDateTime);
        }
    }
}

