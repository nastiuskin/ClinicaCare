using FluentResults;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; private set; }
        public PhoneNumber(string value)
        {
            Value = value;
        }

        public static Result<PhoneNumber> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Fail<PhoneNumber>("Phone number is required.");

            if (!Regex.IsMatch(phoneNumber, @"^\+373\d{8}$"))
                return Result.Fail<PhoneNumber>("Invalid phone number format.");

            return Result.Ok(new PhoneNumber(phoneNumber));
        }
    }
}
