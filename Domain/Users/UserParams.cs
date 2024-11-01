namespace Domain.Users
{
    public record UserParams(string FirstName, string LastName, DateOnly DateOfBirth, string Email, string PhoneNumber);
}
