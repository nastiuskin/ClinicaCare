using Domain.ValueObjects;

namespace Domain.SeedWork
{
    public record UserParams(UserId UserId, string FirstName, string LastName, DateTime DateOfBirth, Email Email, PhoneNumber PhoneNumber);
}
