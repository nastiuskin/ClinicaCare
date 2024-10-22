using Domain.Intefraces;

namespace Domain.MedicalProcedures
{
    public interface IMedicalProcedureRepository : IBaseRepository<MedicalProcedure>
    {
        public Task<MedicalProcedure> GetByIdAsync(MedicalProcedureId id);
    }
}
