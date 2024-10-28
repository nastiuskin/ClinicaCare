using Domain.MedicalProcedures;

namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MedicalProcedureType Type { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
    }
}
