using System.ComponentModel.DataAnnotations;

namespace Application.UserAccountManagement.UserDtos
{
    public class UserFormDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "First Name must be between 1 and 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "Last Name must be between 1 and 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{4}$",
            ErrorMessage = "Date of Birth must be in the format dd.MM.yyyy.")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Compare("Password",
            ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}