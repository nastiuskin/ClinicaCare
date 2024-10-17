using Domain.SeedWork.ValueObjects;

namespace Domain.SeedWork
{
    public record UserParams(int Id, string FirstName, string LastName, DateTime DateOfBirth, Email Email, PhoneNumber PhoneNumber);
}
