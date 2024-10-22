﻿using Domain.MedicalProcedures;
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
                .SingleOrDefaultAsync(mp => mp.Id == id);
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

        public async Task<IEnumerable<MedicalProcedure>> GetAllAsync()
        {
            return await _context.MedicalProcedures
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalProcedure>> GetAllByDoctorIdAsync(UserId doctorId)
        {
            return await _context.MedicalProcedures
                .Where(mp => mp.Doctors
                .Any(d => d.Id == doctorId))
                .ToListAsync();
        }

       

       



    }
}
