using Application.Helpers.PaginationStuff;
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
        public IQueryable<MedicalProcedure?> GetAllProceduresByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize);
        public IQueryable<MedicalProcedure?> GetAllProceduresByTypeAsync(MedicalProcedureType type, int pageNumber, int pageSize);

        public PagedList<MedicalProcedure> GetMedicalProceduresAsync(QueryStringParameters parameters);

    }
}
