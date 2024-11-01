namespace Domain.Users.Doctors
{
    public record DoctorParams(SpecializationType Specialization, string Biography, int CabinetNumber, TimeSpan WorkingHoursStart, TimeSpan WorkingHoursEnd);
}
