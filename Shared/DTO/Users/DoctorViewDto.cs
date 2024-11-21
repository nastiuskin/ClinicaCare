using Application.UserAccountManagement.UserDtos;
using Domain.Users.Doctors;

namespace Shared.DTO.Users
{
    public class DoctorViewDto : UserViewDto
    {
        public SpecializationType SpecializationType { get; set; }
        public string Biography { get; set; }
        public int CabinetNumber { get; set; }
        public string WorkingHoursStart { get; set; }
        public string WorkingHoursEnd { get; set; }
    }
}
