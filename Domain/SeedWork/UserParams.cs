using Domain.ValueObjects;

namespace Domain.SeedWork
{
    public record UserParams(UserId Id, string FirstName, string LastName, DateTime DateOfBirth, Email Email, PhoneNumber PhoneNumber);
}
