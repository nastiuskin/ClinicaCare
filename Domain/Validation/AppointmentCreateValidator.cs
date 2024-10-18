using Domain.Appointments;
using FluentValidation;

namespace Domain.Validation
{
    public class AppointmentCreateValidator : AbstractValidator<Appointment>
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

               RuleFor(appointment => appointment.Status)
                    .IsInEnum().WithMessage("Invalid appointment status.");
            }
    }
}
