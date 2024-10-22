using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Validation;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Users.Doctors
{
    public class Doctor : User
    {
        private List<Appointment> _appointments;
        private List<MedicalProcedure> _medicalProcedures;
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
            WorkingHours = doctorParams.WorkingHours;

            _appointments = new List<Appointment>();
            _medicalProcedures = new List<MedicalProcedure>();
        }

        public static Result<Doctor> Create(UserParams userParams, DoctorParams doctorParams)
        {
            var userValidator = new UserCreateValidator();
            var userValidationResult = userValidator.Validate(userParams);

            if (!userValidationResult.IsValid)
            {
                var errors = userValidationResult.Errors
                    .Select(error => new Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

            var doctorValidator = new DoctorСreateValidator();
            var doctorValidationResult = doctorValidator.Validate(doctorParams);

            if (!doctorValidationResult.IsValid)
            {
                var errors = doctorValidationResult.Errors
                    .Select(error => new Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);
            }

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
