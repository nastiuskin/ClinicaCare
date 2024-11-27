using Application.Helpers.PaginationStuff;
using Domain.MedicalProcedures;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.MedicalProcedures
{
    public class MedicalProcedureRepository : IMedicalProcedureRepository
    {
        private readonly AppDbContext _context;

        public MedicalProcedureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MedicalProcedure entity)
        {
            _context.MedicalProcedures.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<MedicalProcedure?> GetByIdAsync(MedicalProcedureId id)
        {
            return await _context.MedicalProcedures
                .FirstOrDefaultAsync(mp => mp.Id == id);
        }

        public async Task<MedicalProcedure?> GetByIdWithDoctorsAsync(MedicalProcedureId id)
        {
            return await _context.MedicalProcedures
                .Include(mp => mp.Doctors)
                .FirstOrDefaultAsync(mp => mp.Id == id);
        }

        public async Task<MedicalProcedure?> GetByIdWithDoctorsAndAppointmentsAsync(MedicalProcedureId id)
        {
            return await _context.MedicalProcedures
                    .Include(mp => mp.Doctors)
                    .Include(mp => mp.Appointments)
                    .FirstOrDefaultAsync(mp => mp.Id == id);
        }

        public async Task<MedicalProcedure?> GetByNameAsync(string name)
        {
            return await _context.MedicalProcedures.FirstOrDefaultAsync(mp => mp.Name == name);
        }

        public async Task UpdateAsync(MedicalProcedure entity)
        {
            _context.MedicalProcedures.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MedicalProcedure entity)
        {
            _context.MedicalProcedures.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<MedicalProcedure> GetAll()
        {
            return _context.MedicalProcedures;
               
        }

        public IQueryable<MedicalProcedure> GetAllProceduresByDoctorIdAsync(UserId doctorId, int pageNumber, int pageSize)
        {
            return _context.MedicalProcedures
                .Where(mp => mp.Doctors
                .Any(d => d.Id == doctorId));
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.MedicalProcedures.CountAsync();
        }

        public PagedList<MedicalProcedure> GetMedicalProceduresAsync(MedicalProcedureParameters parameters)
        {
            var query = GetAll();

            if (parameters.Type.HasValue)
                query = query.Where(m => m.Type == parameters.Type.Value);

            return PagedList<MedicalProcedure>
                .ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public IQueryable<MedicalProcedure?> GetAllProceduresByTypeAsync(MedicalProcedureType type, int pageNumber, int pageSize)
        {
           return _context.MedicalProcedures.Where(mp => mp.Type == type);
        }
    }
}
