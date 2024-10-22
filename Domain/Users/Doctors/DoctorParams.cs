using Domain.ValueObjects;

namespace Domain.Users.Doctors
{
    public record DoctorParams(SpecializationType Specialization, string Biography, int CabinetNumber, TimeSlot WorkingHours);
}
