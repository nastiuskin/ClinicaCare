using Domain.Intefraces;

namespace Domain.Appointments
{
    public record AppointmentId(Guid Value) : ITypedId;
}
