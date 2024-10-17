using System.ComponentModel.DataAnnotations;

namespace Domain.SeedWork.ValueObjects
{
    public record Email
    {
        public string Value { get; private set; }
        private Email(string value)
        {
            Value=value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(value));
            }

            if (!new EmailAddressAttribute().IsValid(value))
            {
                throw new ArgumentException("Invalid email format.", nameof(value));
            }
            return new Email(value);
        }
    }
}
