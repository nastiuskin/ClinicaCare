using Domain.Intefraces;
using Domain.Users;

namespace Domain.MedicalProcedures
{
    public interface IMedicalProcedureRepository : IBaseRepository<MedicalProcedure>
    {
        public Task<MedicalProcedure?> GetByIdAsync(MedicalProcedureId id);
        public Task<MedicalProcedure?> GetByIdWithDoctorsAsync(MedicalProcedureId id);
        public Task<MedicalProcedure?> GetByIdWithDoctorsAndAppointmentsAsync(MedicalProcedureId id);
        public Task<MedicalProcedure?> GetByNameAsync(string name);
        public Task<IEnumerable<MedicalProcedure>> GetAllByDoctorIdAsync(UserId doctorId);
        public Task<IEnumerable<MedicalProcedure>> GetAllByTypeAsync(MedicalProcedureType type);

    }
}
