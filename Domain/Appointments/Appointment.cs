using Domain.Appointments.Rules;
using Domain.MedicalProcedures;
using Domain.SeedWork;
using Domain.Users;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using Domain.ValueObjects;
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
        public DateOnly Date { get; private set; }
        public TimeSlot Duration { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string DoctorFeedback { get; private set; }

        private Appointment() { }

        private Appointment(UserId doctorId, UserId patientId,
            MedicalProcedureId medicalProcedureId, DateOnly date, TimeSlot duration)
        {
            Id = new AppointmentId(Guid.NewGuid());
            DoctorId = doctorId;
            PatientId = patientId;
            MedicalProcedureId = medicalProcedureId;
            Duration = duration;

            Date = date;
            Status = AppointmentStatus.SCHEDULED;
            DoctorFeedback = string.Empty;
        }

        public Result AddFeedback(string feedback)
        {
            if (String.IsNullOrWhiteSpace(feedback)) return Result.Fail("Feedback cannot be empty");
            var ruleResult = CheckRule(new FeedbackCanBeAddedOnlyIfStatusIsCompletedRule(this));
            if (ruleResult.IsFailed) return ruleResult;

            DoctorFeedback = feedback;
            return Result.Ok();
        }

        public Result Complete()
        {
            Status = AppointmentStatus.COMPLETED;
            return Result.Ok();
        }

        public Result Cancel()
        {
            Status = AppointmentStatus.CANCELED;
            return Result.Ok();
        }

        public static Result<Appointment> Create(UserId doctorId, UserId patientId,
            MedicalProcedureId medicalProcedureId, DateOnly date, TimeSlot duration)
        {
            return Result.Ok(new Appointment(doctorId, patientId, medicalProcedureId, date, duration));
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

