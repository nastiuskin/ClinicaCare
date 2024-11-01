using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Users.Doctors
{
    public class Doctor : User
    {
        private readonly List<Appointment> _appointments;
        private readonly List<MedicalProcedure> _medicalProcedures;
        //public List<MedicalProcedure> MedicalProcedures {  get; private set; }

        public TimeSlot WorkingHours { get; private set; }
        public SpecializationType Specialization { get; private set; }

        public string Biography { get; private set; }
        public int CabinetNumber { get; private set; }

        protected Doctor() { }
        private Doctor(UserParams userParams, DoctorParams doctorParams)
                       : base(userParams)
        {
            Specialization = doctorParams.Specialization;
            Biography = doctorParams.Biography;
            CabinetNumber = doctorParams.CabinetNumber;
            WorkingHours = TimeSlot.Create(doctorParams.WorkingHoursStart, doctorParams.WorkingHoursEnd).Value;

            _appointments = new List<Appointment>();
            _medicalProcedures = new List<MedicalProcedure>();
        }

        public static Result<Doctor> Create(UserParams userParams, DoctorParams doctorParams)
        {
            return Result.Ok(new Doctor(userParams, doctorParams));
        }

        public Result AddAppointment(Appointment appointment)
        {
            if (appointment == null)
                return Result.Fail(new Error("Appointment cannot be null."));

            _appointments.Add(appointment);
            return Result.Ok();
        }

        public Result AddMedicalProcedure(MedicalProcedure medicalProcedure)
        {
            if (medicalProcedure == null)
                return Result.Fail(new Error("Medical procedure cannot be null."));

            _medicalProcedures.Add(medicalProcedure);
            return Result.Ok();
        }

        public IReadOnlyCollection<Appointment?> GetPlannedAppointments()
        {
            return _appointments.Where(a => a.Status == AppointmentStatus.SCHEDULED)
                .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<Appointment?> GetArchivedAppointments()
        {
            return _appointments.Where(a => a.Status == AppointmentStatus.COMPLETED 
                                || a.Status == AppointmentStatus.CANCELED).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();
        public IReadOnlyCollection<MedicalProcedure> MedicalProcedures => _medicalProcedures.AsReadOnly();

    }
}
