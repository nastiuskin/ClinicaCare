using Domain.SeedWork;
using Domain.ValueObjects;
using FluentResults;
namespace Domain.Users
{
    public abstract class User : IAggregateRoot
    {
        public UserId Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public DateOnly DateOfBirth { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }
        protected User() { }

        protected User(UserParams user)
        {
            Id = new UserId(Guid.NewGuid());
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = Email.Create(user.Email).Value;
            PhoneNumber = PhoneNumber.Create(user.PhoneNumber).Value;
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

        //public static Result ValidateUserParams(UserParams userParams)
        //{
        //    var errors = new List<Error>();
        //    if (string.IsNullOrWhiteSpace(userParams.FirstName)) errors.Add(new Error("First name cannot be empty."));

        //    if (string.IsNullOrWhiteSpace(userParams.LastName)) errors.Add(new Error("Last name cannot be empty."));

        //    // Validate email
        //    var emailValidationResult = Email.Create(userParams.Email);
        //    if (emailValidationResult.IsFailed) errors.AddRange((IEnumerable<Error>)emailValidationResult.Errors);

        //    //Validate PhoneNumber
        //    var phoneNumberValidationResult = PhoneNumber.Create(userParams.PhoneNumber);
        //    if (phoneNumberValidationResult.IsFailed) errors.AddRange((IEnumerable<Error>)phoneNumberValidationResult.Errors);

        //    return errors.Count > 0 ? Result.Fail(errors) : Result.Ok();
        //}


        //private static Result<User> Create(UserParams userParams)
        //{
        //    var validator = new UserCreateValidator();
        //    var validationResult = validator.Validate(userParams);
        //    if (!validationResult.IsValid)
        //    {
        //        var errors = validationResult.Errors
        //            .Select(error => new FluentResults.Error(error.ErrorMessage))
        //            .ToList();
        //        return Result.Fail(errors);

        //    }
        //    return Result.Ok(new User(userParams));
        //}

    }

}


