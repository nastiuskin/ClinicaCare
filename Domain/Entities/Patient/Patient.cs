namespace Domain.Entities.Patient
{
    public class Patient : User
    {
        private readonly List<Appointment.Appointment> _appointments;
        private readonly List<MedicalRecord> _medicalHistory;
        public IReadOnlyCollection<Appointment.Appointment> Appointments => _appointments.AsReadOnly();

        public Patient(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
            : base(firstName, lastName, email, phoneNumber)
        {
            _appointments = new List<Appointment.Appointment>();
            _medicalHistory = new List<MedicalRecord>();
        }

        public void AddAppointment(Appointment.Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public void AddRecord(MedicalRecord newRecord)
        {
            _medicalHistory.Add(newRecord);
        }
    }
}
