using Domain.Appointments;
using Domain.MedicalProcedures;
using Domain.Users;
using Persistence.Database.Appointments;
using Persistence.Database.MedicalProcedures;
using Persistence.Database.Users;

namespace API.Extensions
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMedicalProcedureRepository, MedicalProcedureRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            return services;
        }
    }
}
