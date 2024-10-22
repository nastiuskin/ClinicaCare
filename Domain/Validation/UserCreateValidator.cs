using Domain.Users;
using FluentValidation;

namespace Domain.Validation
{
    public class UserCreateValidator : AbstractValidator<UserParams>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Length(1, 100).WithMessage("First Name must be between 1 and 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Length(1, 100).WithMessage("Last Name must be between 1 and 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEqual(default(DateTime)).WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now).WithMessage("Invalid date of birth");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.");

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("Phone number is required.");
        }
    }
}
