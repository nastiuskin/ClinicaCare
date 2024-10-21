using Domain.MedicalProcedures;
using FluentValidation;

namespace Domain.Validation
{
    public class MedicalProcedureCreateValidator : AbstractValidator<MedicalProcedureParams>
    {
        public MedicalProcedureCreateValidator()
        {
            RuleFor(mp => mp.Type)
                .NotNull().WithMessage("Medical procedure type is required.");

            RuleFor(mp => mp.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(mp => mp.Duration)
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be a positive value.");
        }
    }
}

