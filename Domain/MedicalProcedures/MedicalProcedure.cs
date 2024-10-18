using Domain.Doctors;
using Domain.SeedWork;
using FluentResults;
using System.ComponentModel.DataAnnotations;


namespace Domain.MedicalProcedures
{
    public class MedicalProcedure : IAgregateRoot
    {
        private readonly List<Doctor> _doctors;

        public int Id { get; private set; }

        [Required(ErrorMessage = "Procedure type is required.")]
        public MedicalProcedureType Type { get; private set; }

        public decimal Price { get; private set; }


        [Required(ErrorMessage = "Duration is required.")]
        public TimeSpan Duration { get; private set; }

        public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

        private MedicalProcedure(MedicalProcedureType type, decimal price, TimeSpan duration)
        {
            Type = type;
            Price = price;
            Duration = duration;
            _doctors = new List<Doctor>();
        }

        public Result AssignDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return Result.Fail(new FluentResults.Error("Doctor cannot be null."));
            }

            if (_doctors.Contains(doctor))
            {
                return Result.Fail(new FluentResults.Error("Doctor is already in the list."));
            }

            _doctors.Add(doctor);
            return Result.Ok();
        }

        public Result RemoveDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return Result.Fail(new FluentResults.Error("Doctor cannot be null"));
            }

            if (!_doctors.Contains(doctor))
            {
                return Result.Fail(new FluentResults.Error("Doctor not found in the list"));
            }

            _doctors.Remove(doctor);
            return Result.Ok();
        }

        public Result UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                return Result.Fail(new FluentResults.Error("Price must be greater than zero"));
            }

            Price = newPrice;
            return Result.Ok();
        }

        public Result UpdateDuration(TimeSpan newDuration)
        {
            if (newDuration <= TimeSpan.Zero)
            {
                return Result.Fail(new FluentResults.Error("Duration must be a positive value"));
            }

            Duration = newDuration;
            return Result.Ok();
        }

        public static Result<MedicalProcedure> Create(MedicalProcedureType type, decimal price, TimeSpan duration)
        {
            if (price <= 0)
                return Result.Fail(new FluentResults.Error("Price must be greater than zero."));

            if (duration <= TimeSpan.Zero)
                return Result.Fail(new FluentResults.Error("Duration must be a positive value."));

            return Result.Ok(new MedicalProcedure(type, price, duration));
        }
    }
}
