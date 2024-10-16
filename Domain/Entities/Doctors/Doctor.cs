using Domain.Entities.Aggregates.Appointment;
using Domain.Entities.enums;
using Domain.Entities.MedicalServices;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Doctors
{
    public class Doctor : User
    {
        private ICollection<Appointment> _appointments;
        private ICollection<MedicalService> _medicalServices;

        public IReadOnlyCollection<Appointment> GetAppointments() => (IReadOnlyCollection<Appointment>)_appointments;
        public IReadOnlyCollection<MedicalService> GetMedicalServices() => (IReadOnlyCollection<MedicalService>)_medicalServices;


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

            _appointments = new List<Appointment>();
            _medicalServices = new List<MedicalService>();
        }

        public void AddAppointment(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public void AddMedicalService(MedicalService service)
        {
            _medicalServices.Add(service);
        }
    }
}
