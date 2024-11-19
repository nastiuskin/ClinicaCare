using Domain.MedicalProcedures;

namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureUpdateDto
    {
        public string Name { get; set; }
        public MedicalProcedureType Type { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public List<Guid> DoctorsToAdd { get; set; } 
        public List<Guid> DoctorsToRemove { get; set; } 
    }
}
