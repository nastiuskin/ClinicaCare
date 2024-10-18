using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; private set; }
        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Fail(new FluentResults.Error("Email cannot be empty."));

            if (!new EmailAddressAttribute().IsValid(value))
                return Result.Fail(new FluentResults.Error("Invalid email format."));

            return Result.Ok(new Email(value));
        }
    }
}
