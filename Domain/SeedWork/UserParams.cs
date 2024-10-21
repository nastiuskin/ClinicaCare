using Domain.ValueObjects;

namespace Domain.SeedWork
{
    public record UserParams(string FirstName, string LastName, DateTime DateOfBirth, Email Email, PhoneNumber PhoneNumber);
}
