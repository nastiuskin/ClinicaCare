using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace API.Extensions
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
             services.AddIdentity<User, IdentityRole<UserId>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
             return services;
        }
    }
}
