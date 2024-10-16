using Domain.Appointments;
using Domain.MedicalServices;
using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace Domain.Doctors
{
    public class Doctor : User, IAgregateRoot
    {
        private List<Appointment> _appointments;
        private List<MedicalProcedure> _medicalProcedures;
        public TimeSlot WorkingHours { get; private set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public SpecializationType Specialization { get; private set; }

        public string Biography { get; private set; }

        [Required(ErrorMessage = "Cabinet number is required.")]
        public int CabinetNumber { get; private set; }

        public Doctor(string firstName, string lastName, string email, string phoneNumber,
                      SpecializationType specialization, string biography, int cabinetNumber)
                       : base(firstName, lastName, email, phoneNumber)
        {
            Specialization = specialization;
            Biography = biography;
            CabinetNumber = cabinetNumber;

            _appointments = new List<Appointment>();
            _medicalProcedures = new List<MedicalProcedure>();
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "Appointment cannot be null.");
            }
            _appointments.Add(appointment);
        }

        public void AddMedicalProcedure(MedicalProcedure medicalProcedure)
        {
            if (medicalProcedure == null)
            {
                throw new ArgumentNullException(nameof(medicalProcedure), "Medical procedure cannot be null.");
            }
            _medicalProcedures.Add(medicalProcedure);
        }

        public IReadOnlyCollection<Appointment> GetPlannedAppointments()
        {
            return _appointments.Where(a => a.Status == AppointmentStatus.SCHEDULED).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<Appointment> GetArchivedAppointments()
        {
            return _appointments.Where(a => a.Status == AppointmentStatus.COMPLETED || a.Status == AppointmentStatus.CANCELED).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();
        public IReadOnlyCollection<MedicalProcedure> MedicalProcedures => _medicalProcedures.AsReadOnly();

    }
}
