using System.Text.RegularExpressions;

namespace Domain.SeedWork.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; private set; }
        public PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.");
            if (!Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$"))
                throw new ArgumentException("Invalid phone number format.");
            return new PhoneNumber(phoneNumber);
        }
    }
}
