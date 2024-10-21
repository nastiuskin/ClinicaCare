using Domain.Appointments;
using Domain.SeedWork;
using FluentResults;

namespace Domain.Patients
{
    public class Patient : User, IAgregateRoot
    {
        private readonly List<Appointment> _appointments;
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        protected Patient() { }
        public Patient(UserParams userParams)
            : base(userParams)
        {
            _appointments = new List<Appointment>();
        }

        public Result AddAppointment(Appointment appointment)
        {
            if (appointment == null)
                return Result.Fail(new FluentResults.Error("Appointment cannot be null"));

            _appointments.Add(appointment);
            return Result.Ok();
        }

    }
}
