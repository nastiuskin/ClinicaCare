using Domain.MedicalProcedures;

namespace Application.MedicalProcedureManagement.Queries.QueryObjects
{
    public class MedicalProcedureInfoDto
    {
        //doctors??
        public Guid Id { get; set; }
        public MedicalProcedureType Type { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
    }
}
