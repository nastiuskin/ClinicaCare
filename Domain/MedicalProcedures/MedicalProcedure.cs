using Domain.Appointments;
using Domain.SeedWork;
using Domain.Users.Doctors;
using FluentResults;

namespace Domain.MedicalProcedures
{
    public class MedicalProcedure : IAggregateRoot
    {
        public MedicalProcedureId Id { get; private set; }

        public string Name { get; private set; }

        private readonly List<Doctor?> _doctors;

        private readonly List<Appointment?> _appointments;
        public MedicalProcedureType Type { get; private set; }

        public decimal Price { get; private set; }

        public TimeSpan Duration { get; private set; }

        public IReadOnlyCollection<Doctor?> Doctors => _doctors.AsReadOnly();
        public IReadOnlyCollection<Appointment?> Appointments => _appointments.AsReadOnly();

        private MedicalProcedure()
        {
            _doctors = new List<Doctor?>();
            _appointments = new List<Appointment?>();
        }
        private MedicalProcedure(MedicalProcedureType type, decimal price, TimeSpan duration, string name)
        {
            Id = new MedicalProcedureId(Guid.NewGuid()); //??
            Name = name;
            Type = type;
            Price = price;
            Duration = duration;
            _doctors = new List<Doctor?>();
            _appointments = new List<Appointment?>();
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

        public static Result<MedicalProcedure> Create(MedicalProcedureType type, decimal price, TimeSpan duration, string name)
        {
            return Result.Ok(new MedicalProcedure(type, price, duration, name));
        }
    }
}
