using Domain.Doctors;
using Domain.SeedWork;
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

        public void AssignDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            if (!_doctors.Contains(doctor))
            {
                _doctors.Add(doctor);
            }
        }

        public void RemoveDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            if (!_doctors.Contains(doctor))
            {
                throw new InvalidOperationException("Doctor not found in the list.");
            }

            _doctors.Remove(doctor);
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));
            }

            Price = newPrice;
        }

        public void UpdateDuration(TimeSpan newDuration)
        {
            if (newDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value.", nameof(newDuration));
            }

            Duration = newDuration;
        }

        public static MedicalProcedure Create(MedicalProcedureType type, decimal price, TimeSpan duration)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("Duration must be a positive value.", nameof(duration));

            return new MedicalProcedure(type, price, duration);
        }
    }
}
