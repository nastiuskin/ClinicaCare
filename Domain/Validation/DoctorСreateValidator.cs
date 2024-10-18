using Domain.Doctors;
using FluentValidation;

namespace Domain.Validation
{
    public class DoctorСreateValidator : AbstractValidator<DoctorParams>
    {
        public DoctorСreateValidator()
        {
            RuleFor(doctor => doctor.Specialization)
                .NotEmpty().WithMessage("Specialization is required.");

            RuleFor(doctor => doctor.Biography)
                .NotEmpty().WithMessage("Biography is required")
                    .MaximumLength(500).WithMessage("Biography cannot exceed 500 characters.");

            RuleFor(doctor => doctor.CabinetNumber)
                .NotEmpty().WithMessage("Cabinet number is required.")
                    .GreaterThan(0).WithMessage("Cabinet number must be greater than zero.");

            RuleFor(doctor => doctor.WorkingHours)
                .NotNull().WithMessage("Working hours are required.");

        }
    }
}
