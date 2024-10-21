using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Domain.Validation;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Appointments
{
    public class Appointment : IAgregateRoot
    {
        public AppointmentId Id { get; private set; }
        
        public UserId DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public UserId PatientId { get; private set; }
        public Patient Patient { get; private set; }

        public MedicalProcedureId MedicalProcedureId { get; private set; }
        public MedicalProcedure MedicalProcedure { get; private set; }
        public TimeSlot AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        private Appointment() { }

        private Appointment(AppointmentParams appointmentParams)
        {
            Doctor = appointmentParams.Doctor;
            Patient = appointmentParams.Patient;
            MedicalProcedure = appointmentParams.MedicalProcedure;

            AppointmentDateTime = appointmentParams.AppointmentDateTime;
            Status = AppointmentStatus.SCHEDULED;
            DoctorFeedback = string.Empty;
        }

        //To create Bussiness Rule for this
        public Result AddFeedback(string feedback)
        {
            if (Status != AppointmentStatus.COMPLETED)
                return Result.Fail(new FluentResults.Error("Feedback can only be added after the appointment is completed."));

            if (string.IsNullOrWhiteSpace(feedback))
                return Result.Fail(new FluentResults.Error("Feedback cannot be empty."));

            DoctorFeedback = feedback;
            return Result.Ok();
        }

        public Result UpdateStatus(AppointmentStatus newStatus)
        {
            Status = newStatus;
            return Result.Ok();
        }


        public static Result<Appointment> Create(AppointmentParams appointmentParams)
        {
            var validator = new AppointmentCreateValidator();
            var appointmentValidatorResult = validator.Validate(appointmentParams);

            if (!appointmentValidatorResult.IsValid)
            {
                var errors = appointmentValidatorResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            return Result.Ok(new Appointment(appointmentParams));
        }
    }
}

