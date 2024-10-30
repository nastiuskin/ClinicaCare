using Application.UserAccountManagement.Doctors.DTO;
using FluentValidation;

namespace Domain.Validation
{
    public class DoctorСreateDtoValidator : AbstractValidator<DoctorFormDto>
    {
        public DoctorСreateDtoValidator()
        {
            Include(new UserCreateDtoValidator()); 

            RuleFor(doctor => doctor.Specialization)
                .NotEmpty().WithMessage("Specialization is required.");

            RuleFor(doctor => doctor.Biography)
                .NotEmpty().WithMessage("Biography is required")
                    .MaximumLength(500).WithMessage("Biography cannot exceed 500 characters.");

            RuleFor(doctor => doctor.CabinetNumber)
                .NotEmpty().WithMessage("Cabinet number is required.")
                    .GreaterThan(0).WithMessage("Cabinet number must be greater than zero.");

            RuleFor(doctor => doctor.WorkingHoursStart)
                .NotEmpty().WithMessage("Working hours start is required.")
                .Matches(@"^\d{1,2}:[0-5][0-9]$").WithMessage("Working hours start must be in the format hh:mm");

            RuleFor(doctor => doctor.WorkingHoursEnd)
                .NotEmpty().WithMessage("Working hours end is required.")
                .Matches(@"^\d{1,2}:[0-5][0-9]$").WithMessage("Working hours start must be in the format hh:mm")
                    .GreaterThan(doctor => doctor.WorkingHoursStart);
        }
    }
}
