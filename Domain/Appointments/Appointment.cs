﻿using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Appointments
{
    public class Appointment : IAgregateRoot
    {
        public AppointmentId AppointmentId { get; private set; }

        public DoctorId DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public PatientId PatientId { get; private set; }
        public Patient Patient { get; private set; }


        public MedicalProcedureId MedicalProcedureId { get; private set; }
        public MedicalProcedure MedicalProcedure { get; private set; }

        public TimeSlot AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        protected Appointment() { }

        private Appointment(Doctor doctor, Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
        {
            Doctor = doctor;
            Patient = patient;
            MedicalProcedure = medicalProcedure;

            AppointmentDateTime = appointmentDateTime;
            Status = AppointmentStatus.SCHEDULED;
            DoctorFeedback = string.Empty;
        }

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


        //AppointmentValidator needed
        public static Appointment Create(Doctor doctor, Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
        {
            if (doctor == null) throw new ArgumentNullException("Doctor cannot be null", nameof(doctor));
            if (patient == null) throw new ArgumentNullException("Patient cannot be null", nameof(patient));
            if (medicalProcedure == null) throw new ArgumentNullException("Medical Procedure cannot be null", nameof(medicalProcedure));
            if (appointmentDateTime == null) throw new ArgumentNullException("Appointment time is required", nameof(appointmentDateTime));
            return new Appointment(doctor, patient, medicalProcedure, appointmentDateTime);
        }

    //AppointmentValidator needed
    /*public static Result<Appointment> Create(Doctor doctor, Patient patient, MedicalProcedure medicalProcedure, TimeSlot appointmentDateTime)
        {
            var validator = new AppointmentCreateValidator();
            var appointment = new Appointment(doctor, patient, medicalProcedure, appointmentDateTime);
            var appointmentValidatorResult = validator.Validate(appointment);

            if (!appointmentValidatorResult.IsValid)
            {
                var errors = appointmentValidatorResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            return Result.Ok(appointment);
        }*/
    }
}

