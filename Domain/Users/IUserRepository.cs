using Application.Helpers.PaginationStuff;
using Domain.Helpers.PaginationStuff;
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
        public PagedList<Doctor> GetAllDoctorsAsync(UserParameters parameters); 
        public PagedList<Patient> GetAllPatientsAsync(UserParameters parameters);  
        public IQueryable<Doctor> GetByIdWithAppointmentsOnSpecificDayAsync(UserId doctorId, DateOnly date);
    }
}
