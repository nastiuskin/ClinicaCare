using Domain.Intefraces;
using Domain.Users.Doctors;
using Domain.Users.Patients;

namespace Domain.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> GetByIdAsync(UserId id);
        public Task<IEnumerable<Doctor>> GetAllDoctorsAsync(int pageNumber, int pageSize);
        public Task<IEnumerable<Patient>> GetAllPatientsAsync(int pageNumber, int pageSize);
    }
}
