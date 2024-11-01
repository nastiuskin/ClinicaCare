using Application.MedicalProcedureManagement.DTO;
using FluentValidation;

namespace Domain.Validation
{
    public class MedicalProcedureCreateDtoValidator : AbstractValidator<MedicalProcedureFormDto>
    {
        public MedicalProcedureCreateDtoValidator()
        {
            RuleFor(mp => mp.Name)
               .NotNull()
               .NotEmpty().WithMessage("Medical procedure name is required.");

            RuleFor(mp => mp.Type)
                .NotNull().WithMessage("Medical procedure type is required.");

            RuleFor(mp => mp.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(mp => mp.Duration)
            .NotEmpty().WithMessage("Duration is required")
             .Matches(@"^\d{1,2}:[0-5][0-9]$").WithMessage("Duration must be in the format hh:mm");
        }
    }
}

