using Domain.Appointments.Rules;
using Domain.MedicalProcedures;
using Domain.SeedWork;
using Domain.Users;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using FluentResults;

namespace Domain.Appointments
{
    public class Appointment : IAggregateRoot
    {
        public AppointmentId Id { get; private set; }

        public UserId DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public UserId PatientId { get; private set; }
        public Patient Patient { get; private set; }

        public MedicalProcedureId MedicalProcedureId { get; private set; }
        public MedicalProcedure MedicalProcedure { get; private set; }
        public DateTime AppointmentDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        private Appointment() { }

        private Appointment(UserId doctorId, UserId patientId,
            MedicalProcedureId medicalProcedureId, DateTime appointmentDateTime)
        {
            Id = new AppointmentId(Guid.NewGuid());
            DoctorId = doctorId;
            PatientId = patientId;
            MedicalProcedureId = medicalProcedureId;

            AppointmentDateTime = appointmentDateTime;
            Status = AppointmentStatus.SCHEDULED;
            DoctorFeedback = string.Empty;
        }

        public Result AddFeedback(string feedback)
        {
            var ruleResult = CheckRule(new FeedbackCanBeAddedOnlyIfStatusIsCompletedRule(this));
            if (ruleResult.IsFailed) return ruleResult;

            DoctorFeedback = feedback;
            return Result.Ok();
        }

        public Result UpdateStatus(AppointmentStatus newStatus)
        {
            Status = newStatus;
            return Result.Ok();
        }

        public static Result<Appointment> Create(UserId doctorId, UserId patientId,
            MedicalProcedureId medicalProcedureId, DateTime appointmentDateTime)
        {
            return Result.Ok(new Appointment(doctorId, patientId, medicalProcedureId, appointmentDateTime));
        }

        public Result CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                return Result.Fail(rule.Message);
            }
            return Result.Ok();
        }
    }
}

