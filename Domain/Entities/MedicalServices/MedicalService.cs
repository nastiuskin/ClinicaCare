using Domain.Entities.Doctors;
using Domain.Entities.enums;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.MedicalServices
{
    public class MedicalService
    {
        private readonly List<Doctor> _doctors;

        public int Id { get; private set; }


        [Required(ErrorMessage = "Service type is required.")]
        public ServiceType Type { get; private set; }

        public decimal Price { get; private set; }


        [Required(ErrorMessage = "Duration is required.")]
        public TimeSpan Duration { get; private set; }

        public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

        public MedicalService(ServiceType type, decimal price, TimeSpan duration)
        {
            Type = type;
            Price = price;
            Duration = duration;
            _doctors = new List<Doctor>();
        }

        public void AssignDoctor(Doctor doctor)
        {
            if (!_doctors.Contains(doctor))
            {
                _doctors.Add(doctor);
            }
        }

        public void RemoveDoctor(Doctor doctor)
        {
            _doctors.Remove(doctor);
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0.01m) //no negative value
            {
                throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));
            }
            Price = newPrice;
        }

        public void UpdateDuration(TimeSpan newDuration)
        {
            Duration = newDuration;
        }

    }
}
