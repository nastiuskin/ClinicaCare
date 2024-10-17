<<<<<<<< HEAD:Domain/Entities/Patients/Patient.cs
﻿namespace Domain.Entities.Patient
========
﻿using Domain.Appointments;
using Domain.SeedWork;

namespace Domain.Patient
>>>>>>>> origin/Domain:Domain/Patients/Patient.cs
{
    public class Patient : User, IAgregateRoot
    {
        private readonly List<Appointment> _appointments;
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        public Patient(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
            : base(firstName, lastName, email, phoneNumber)
        {
            _appointments = new List<Appointment>();
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "Appointment cannot be null.");
            }
            _appointments.Add(appointment);
        }

    }
}
