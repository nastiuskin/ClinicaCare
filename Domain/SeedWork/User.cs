using Domain.SeedWork.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.SeedWork
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        protected User(UserParams user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DateOfBirth = user.DateOfBirth;
        }

        protected void ChangePhoneNumber(string phoneNumber)
        {
            PhoneNumber newPhoneNumber = PhoneNumber.Create(phoneNumber);
            PhoneNumber = newPhoneNumber;
        }

        protected void ChangeEmail(string email)
        {
            Email newEmail = Email.Create(email);
            Email = newEmail;

        }

        private static User Create(UserParams userParams)
        {
            if (string.IsNullOrWhiteSpace(userParams.FirstName))
                throw new ArgumentException("First Name is required.", nameof(userParams.FirstName));
            if (string.IsNullOrWhiteSpace(userParams.LastName))
                throw new ArgumentException("Last Name is required.", nameof(userParams.LastName));
            if (userParams.DateOfBirth == default)
                throw new ArgumentException("Date of Birth is required.", nameof(userParams.DateOfBirth));
            return new User(userParams);
        }
    }
}




