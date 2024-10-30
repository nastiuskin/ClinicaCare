using Domain.Intefraces;

namespace Domain.MedicalProcedures
{
    public record MedicalProcedureId(Guid Value) : ITypedId;
}
