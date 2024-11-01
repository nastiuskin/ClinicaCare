using Domain.MedicalProcedures;
namespace Application.MedicalProcedureManagement.DTO
{
    public class MedicalProcedureFormDto
    {
        public string Name { get; set; }
        public MedicalProcedureType Type { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public List<Guid> Doctors { get; set; }
    }
}
