using Domain.Entities.enums;
using Domain.Entities.MedicalServices;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Doctors
{
    public class Doctor : User
    {
        private ICollection<Appointment.Appointment> _appointments;
        private ICollection<MedicalService> _medicalServices;

        public Schedule Schedule { get; private set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public SpecializationType Specialization { get; private set; }

        public string Biography { get; private set; }

        [Required]
        public int CabinetNumber { get; private set; }

        public Doctor(string firstName, string lastName, string email, string phoneNumber,
                      SpecializationType specialization, string biography, int cabinetNumber)
                       : base(firstName, lastName, email, phoneNumber)
        {
            Specialization = specialization;
            Biography = biography;
            CabinetNumber = cabinetNumber;

            _appointments = new List<Appointment.Appointment>();
            _medicalServices = new List<MedicalService>();
        }

        public void AddAppointment(Appointment.Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "Appointment cannot be null.");
            }
            _appointments.Add(appointment);
        }

        public void AddMedicalService(MedicalService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Medical service cannot be null.");
            }
            _medicalServices.Add(service);
        }

        public IReadOnlyCollection<Appointment.Appointment> GetAppointments() => (IReadOnlyCollection<Appointment.Appointment>)_appointments;
        public IReadOnlyCollection<MedicalService> GetMedicalServices() => (IReadOnlyCollection<MedicalService>)_medicalServices;

    }
}
