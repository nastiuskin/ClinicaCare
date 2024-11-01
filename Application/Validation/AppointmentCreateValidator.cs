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
                 .NotNull().WithMessage("Appointment date is required.");

            RuleFor(appointment => appointment.Duration)
                .NotNull().WithMessage("Appointment duration is required");
        }
    }
}
