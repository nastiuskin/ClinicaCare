using Domain.Users;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using Microsoft.EntityFrameworkCore;

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

        public IQueryable<Doctor> GetAllDoctorsAsync(int pageNumber, int pageSize)
        {
            return _context.Users
                .OfType<Doctor>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public IQueryable<Patient> GetAllPatientsAsync(int pageNumber, int pageSize)
        {
            return _context.Users.OfType<Patient>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public IQueryable<User> GetAllAsync(int pageNumber, int pageSize)
        {
            return _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
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

        public IQueryable<Doctor> GetByIdWithAppointmentsAsync(UserId doctorId)
        {
            return _context.Users
                .OfType<Doctor>()
                .Where(d => d.Id == doctorId)
                .Include(d => d.Appointments);
        }
    }
}
