namespace Domain.MedicalProcedures
{
    public record MedicalProcedureParams(MedicalProcedureType Type, decimal Price, TimeSpan Duration);
}
