using Application.AppointmentManagement.DTO;
using FluentValidation;

namespace Application.Validation
{
    public class AppointmentCreateValidator : AbstractValidator<AppointmentFormDto>
    {
        public AppointmentCreateValidator()
        {
            RuleFor(appointment => appointment.DoctorId)
                .NotNull().WithMessage("Doctor cannot be null.");

            RuleFor(appointment => appointment.PatientId)
                .NotNull().WithMessage("Patient cannot be null.");

            RuleFor(appointment => appointment.MedicalProcedureId)
                .NotNull().WithMessage("Medical Procedure cannot be null.");

            RuleFor(appointment => appointment.Date)
                .NotEmpty().WithMessage("Appointment date is required.");

            RuleFor(appointment => appointment.Duration.StartTime)
                .NotNull().WithMessage("Appointment start time is required.");

            RuleFor(appointment => appointment.Duration.EndTime)
                .NotNull().WithMessage("Appointment end time is required.");
        }
    }
}

