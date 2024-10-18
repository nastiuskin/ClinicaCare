using Domain.ValueObjects;

namespace Domain.Doctors
{
    public record DoctorParams(SpecializationType Specialization, string Biography, int CabinetNumber, TimeSlot WorkingHours);
}
