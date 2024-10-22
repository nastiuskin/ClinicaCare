using Domain.Intefraces;

namespace Domain.Users
{
    public record UserId(Guid Value) : ITypedId;

}
