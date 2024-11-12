using Application.UserAccountManagement.UserDtos;
using Domain.Users.Doctors;

namespace Application.UserAccountManagement.Doctors.DTO
{
    public class DoctorFormDto : UserFormDto
    {
        public SpecializationType Specialization {  get; set; }
        public string Biography { get; set; }
        public int CabinetNumber { get; set; }
        public string WorkingHoursStart { get; set; }
        public string WorkingHoursEnd {  get; set; }
    }
}
