using Domain.ValueObjects;

namespace Domain.Users
{
    public record UserParams(string FirstName, string LastName, DateTime DateOfBirth, Email Email, PhoneNumber PhoneNumber);
}
