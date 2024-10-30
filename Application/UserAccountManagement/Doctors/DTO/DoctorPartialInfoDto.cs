using Domain.Users.Doctors;

namespace Application.UserAccountManagement.Doctors.DTO
{
    public class DoctorPartialInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SpecializationType SpecializationType { get; set; }
    }
}
