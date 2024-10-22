using Domain.Intefraces;
using Domain.Users.Doctors;
using Domain.Users.Patients;

namespace Domain.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> GetByIdAsync(UserId id);
        public Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        public Task<IEnumerable<Patient>> GetAllPatientsAsync();
    }
}
