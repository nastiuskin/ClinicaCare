using Application.Auth;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Database.Seed;

namespace API.Extensions
{
    public static class SeedConfiguration
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<UserId>>>();

                await RoleSeeder.SeedRoles(roleManager);
                await SeedAdminAsync.SeedAsync(userManager, roleManager);
            }
        }
    }
}
