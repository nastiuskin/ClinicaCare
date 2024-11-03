using Domain.Intefraces;
using Domain.Users.Doctors;
using Domain.Users.Patients;

namespace Domain.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> GetByIdAsync(UserId id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<IEnumerable<Doctor>> GetListOfDoctorsByIdsAsync(List<UserId> doctors);
        public IQueryable<Doctor> GetAllDoctorsAsync(int pageNumber, int pageSize); //later can be filtered by medical procedure type 
        public IQueryable<Patient> GetAllPatientsAsync(int pageNumber, int pageSize);  //can be filtered by date of birth
        public IQueryable<Doctor> GetByIdWithAppointmentsAsync(UserId doctorId);
    }
}
