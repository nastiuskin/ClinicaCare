using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddDbContext<AppDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

           return services;
        }
       
    }
}
