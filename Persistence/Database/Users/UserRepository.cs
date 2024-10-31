using Domain.Users;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Persistence.Database.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(UserId id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value.Equals(email));
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .OfType<Doctor>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Users
                .OfType<Doctor>()
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Users
                .OfType<Patient>()
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize)
        {
            return await _context.Users.OfType<Patient>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<Doctor>> GetListOfDoctorsByIdsAsync(List<UserId> doctors)
        {
            return await _context.Users
            .OfType<Doctor>()
            .Where(d => doctors.Contains(d.Id))
            .ToListAsync();
        }
    }
}
