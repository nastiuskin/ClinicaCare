using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.SeedWork
{
    public abstract class User
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; private set; }


        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; private set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; private set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; private set; }


        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; private set; }

        protected User(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void ChangePhoneNumber(string phoneNumber)
        {
            var regex = new Regex(@"^\+?[1-9]\d{1,14}$");
            if (!regex.IsMatch(phoneNumber))
            {
                throw new ArgumentException("Invalid phone number format.");
            }
            PhoneNumber = phoneNumber;
        }

        public void ChangeEmail(string newEmail)
        {
            var emailAttribute = new EmailAddressAttribute();

            if (!emailAttribute.IsValid(newEmail))
            {
                throw new ArgumentException("Invalid email address format.");
            }
            Email = newEmail;
        }


        //constructor is private; for object creation we use create method


        //public User Create(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
        //{
        //    if (string.IsNullOrWhiteSpace(firstName))
        //        throw new ArgumentException("First Name is required.", nameof(firstName));
        //    if (string.IsNullOrWhiteSpace(lastName))
        //        throw new ArgumentException("Last Name is required.", nameof(lastName));
        //    if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
        //        throw new ArgumentException("Invalid email address format.", nameof(email));
        //    if (!new Regex(@"^\+?[1-9]\d{1,14}$").IsMatch(phoneNumber))
        //        throw new ArgumentException("Invalid phone number format.", nameof(phoneNumber));
        //    if (dateOfBirth == default(DateTime))
        //        throw new ArgumentException("Date of Birth is required.", nameof(dateOfBirth));
        //    return new User(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth);
        //}
    }


}


