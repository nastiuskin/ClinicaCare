using Domain.Appointments;
using Domain.Doctors;
using Domain.SeedWork;
using Domain.Validation;
using FluentResults;
using System.ComponentModel.DataAnnotations;


namespace Domain.MedicalProcedures
{
    public class MedicalProcedure : IAgregateRoot
    {
        private readonly List<Doctor> _doctors;

        private readonly List<Appointment> _appointments;
        public MedicalProcedureId Id { get; private set; }
        public MedicalProcedureType Type { get; private set; }

        public decimal Price { get; private set; }

        public TimeSpan Duration { get; private set; }

        public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        private MedicalProcedure() { }
        private MedicalProcedure(MedicalProcedureParams mpParams)
        {
            Type = mpParams.Type;
            Price = mpParams.Price;
            Duration = mpParams.Duration;
            _doctors = new List<Doctor>();
        }

        public Result AssignDoctor(Doctor doctor)
        {
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor cannot be null."));

            if (_doctors.Contains(doctor))
                return Result.Fail(new FluentResults.Error("Doctor is already in the list."));

            _doctors.Add(doctor);
            return Result.Ok();
        }

        public Result RemoveDoctor(Doctor doctor)
        {
            if (doctor == null)
                return Result.Fail(new FluentResults.Error("Doctor cannot be null"));

            if (!_doctors.Contains(doctor))
                return Result.Fail(new FluentResults.Error("Doctor not found in the list"));

            _doctors.Remove(doctor);
            return Result.Ok();
        }

        public Result UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                return Result.Fail(new FluentResults.Error("Price must be greater than zero"));

            Price = newPrice;
            return Result.Ok();
        }

        public Result UpdateDuration(TimeSpan newDuration)
        {
            if (newDuration <= TimeSpan.Zero)
                return Result.Fail(new FluentResults.Error("Duration must be a positive value"));

            Duration = newDuration;
            return Result.Ok();
        }

        public static Result<MedicalProcedure> Create(MedicalProcedureParams mpParams)
        {
            var mpValidator = new MedicalProcedureCreateValidator();
            var mpValidationResult = mpValidator.Validate(mpParams);
            if (!mpValidationResult.IsValid)
            {
                var errors = mpValidationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            return Result.Ok(new MedicalProcedure(mpParams));
        }
    }
}
