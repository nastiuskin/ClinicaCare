using Domain.Entities.Aggregates.Appointment;

namespace Domain.Entities.Aggregates.Appointment.Patient
{
    public class Patient : User
    {
        private readonly List<Appointment> _appointments;
        private readonly List<MedicalRecord> _medicalHistory;
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        public Patient(string firstName, string lastName, string email, string password, string phoneNumber, DateTime dateOfBirth)
            : base(firstName, lastName, email, phoneNumber)
        {
            _appointments = new List<Appointment>();
            _medicalHistory = new List<MedicalRecord>();
        }

        public void AddAppointment(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public void AddRecord(MedicalRecord newRecord)
        {
            _medicalHistory.Add(newRecord);
        }
    }
}
