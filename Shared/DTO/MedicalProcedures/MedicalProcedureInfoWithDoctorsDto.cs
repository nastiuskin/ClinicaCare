using Application.UserAccountManagement.Doctors.DTO;
using Domain.MedicalProcedures;
using Domain.Users.Doctors;

namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureInfoWithDoctorsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public List<DoctorPartialInfoDto> Doctors { get; set; }
    }
}
