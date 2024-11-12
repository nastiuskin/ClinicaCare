using Application.UserAccountManagement.UserDtos;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validation
{
    public class UserCreateDtoValidator : AbstractValidator<UserFormDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Length(1, 100).WithMessage("First Name must be between 1 and 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Length(1, 100).WithMessage("Last Name must be between 1 and 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .Matches(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{4}$").WithMessage("Date of Birth must be in the format dd.MM.yyyy.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(email => Email.Create(email).IsSuccess).WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(phoneNumber => PhoneNumber.Create(phoneNumber).IsSuccess).WithMessage("Invalid pnoheNumber format.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
