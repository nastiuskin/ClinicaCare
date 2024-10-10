
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public abstract class User
    {
        public int Id { get; private set; }


        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; private set; }


        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; private set; }


        [Required]
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
            //what about validation here?
           PhoneNumber = phoneNumber;
        }

        public void ChangeEmail(string newEmail)
        {
           //validation??
            Email = newEmail;
        }
    }
}
