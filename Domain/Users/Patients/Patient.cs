using Domain.Appointments;
using Domain.Validation;
using FluentResults;

namespace Domain.Users.Patients
{
    public class Patient : User
    {
        private readonly List<Appointment> _appointments;
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        protected Patient() { }
        private Patient(UserParams userParams)
            : base(userParams)
        {
            _appointments = new List<Appointment>();
        }

        public Result AddAppointment(Appointment appointment)
        {
            if (appointment == null)
                return Result.Fail(new Error("Appointment cannot be null"));

            _appointments.Add(appointment);
            return Result.Ok();
        }

        public Result<Patient> Create(UserParams patientParams)
        {
            var validator = new UserCreateValidator();
            var validationResult = validator.Validate(patientParams);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);

            }
            return Result.Ok(new Patient(patientParams));
        }

    }
}
