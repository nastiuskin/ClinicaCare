using Application.Helpers.PaginationStuff;
using Domain.Helpers.PaginationStuff;
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

        public IQueryable<User> GetAll()
        {
            return _context.Users.OfType<User>();
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
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

        public PagedList<Doctor> GetAllDoctorsAsync(DoctorParameters parameters)
        {
            var query = GetAll().OfType<Doctor>();

            return PagedList<Doctor>
                .ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<Patient> GetAllPatientsAsync(UserParameters parameters)
        {
            return PagedList<Patient>
                 .ToPagedList(GetAll().OfType<Patient>(), parameters.PageNumber, parameters.PageSize);
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

        public IQueryable<Doctor> GetByIdWithAppointmentsOnSpecificDayAsync(UserId doctorId, DateOnly date)
        {
            return _context.Users
                .OfType<Doctor>()
                .Where(d => d.Id == doctorId)
                .Include(d => d.Appointments.Where(a => a.Date == date));
        }
    }
}
