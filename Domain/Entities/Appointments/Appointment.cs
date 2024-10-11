﻿using Domain.Entities.Appointments;
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
        public TimeSlot AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string Notes { get; private set; }

        //is needed create method vor validating appointment creation calling methods isAvailableTime etc
        public Appointment(Doctor doctor, Patient.Patient patient, MedicalService service, TimeSlot appointmentDateTime)
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

        public void Reschedule(TimeSlot newDateTime)
        {
            AppointmentDateTime = newDateTime;
        }
    }
}
