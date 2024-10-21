using Domain.Appointments;
using FluentValidation;

namespace Domain.Validation
{
    public class AppointmentCreateValidator : AbstractValidator<AppointmentParams>
    {
        public AppointmentCreateValidator() 
        {
               RuleFor(appointment => appointment.Doctor)
                   .NotNull().WithMessage("Doctor cannot be null.");

               RuleFor(appointment => appointment.Patient)
                    .NotNull().WithMessage("Patient cannot be null.");

               RuleFor(appointment => appointment.MedicalProcedure)
                    .NotNull().WithMessage("Medical Procedure cannot be null.");

               RuleFor(appointment => appointment.AppointmentDateTime)
                    .NotNull().WithMessage("Appointment time is required.");
            }
    }
}
