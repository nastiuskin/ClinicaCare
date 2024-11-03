using Application.AppointmentManagement.DTO;
using Domain.Appointments;
using FluentValidation;

namespace Domain.Validation
{
    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
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
                 .NotEmpty().WithMessage("Appointment date is required.")
                 .Matches(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{4}$").WithMessage("Date of Birth must be in the format dd.MM.yyyy.");

            RuleFor(appointment => appointment.StartTime)
                .NotNull().WithMessage("Appointment start time is required");

            RuleFor(appointment => appointment.EndTime)
                .NotNull().WithMessage("Appointment start time is required");
        }
    }
}
