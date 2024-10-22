using Domain.Intefraces;
using Domain.Users;

namespace Domain.MedicalProcedures
{
    public interface IMedicalProcedureRepository : IBaseRepository<MedicalProcedure>
    {
        public Task<MedicalProcedure?> GetByIdAsync(MedicalProcedureId id);
        public Task<IEnumerable<MedicalProcedure>> GetAllByDoctorIdAsync(UserId doctorId);
    }
}
