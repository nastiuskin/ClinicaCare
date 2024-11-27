using Domain.MedicalProcedures;

namespace Application.Helpers.PaginationStuff
{
    public class MedicalProcedureParameters : QueryStringParameters
    {
        public MedicalProcedureType? Type { get; set; }
    }
}
