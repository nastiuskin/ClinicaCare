using Domain.Validation;
using Domain.ValueObjects;
using FluentResults;
using System.ComponentModel.DataAnnotations;
namespace Domain.SeedWork
{
    public class User
    {
        public UserId Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; private set; }


        public Email Email { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }
        public User() { }

        protected User(UserParams user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DateOfBirth = user.DateOfBirth;
        }

        protected Result ChangePhoneNumber(string phoneNumber)
        {
            var result = PhoneNumber.Create(phoneNumber);
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            PhoneNumber = result.Value;
            return Result.Ok();
        }

        protected Result ChangeEmail(string email)
        {
            var result = Email.Create(email);
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            Email = result.Value;
            return Result.Ok();

        }

        private static Result<User> Create(UserParams userParams)
        {
           var validator = new UserCreateValidator();
           var validationResult = validator.Validate(userParams);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new FluentResults.Error(error.ErrorMessage))
                    .ToList();
                return Result.Fail(errors);

            }
            return Result.Ok(new User(userParams));
        }
    }
}




