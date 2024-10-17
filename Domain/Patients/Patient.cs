using Domain.Appointments;
using Domain.SeedWork;
using Domain.SeedWork.ValueObjects;

namespace Domain.Patients
{
    public class Patient : User, IAgregateRoot
    {
        private readonly List<Appointment> _appointments;
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        public Patient(UserParams userParams)
            : base(userParams)
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
