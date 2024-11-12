using Domain.Users;
using Domain.Users.Admins;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Persistence.Database.Seed
{
    public class SeedAdminAsync
    {
        public static async Task<IdentityResult> SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<UserId>> roleManager)
        {
            var adminEmail = "admin1@example.com";
            var adminPassword = "admin1Password!";
            var adminRole = "Admin";

            if (await userManager.FindByEmailAsync(adminEmail) != null)
                return IdentityResult.Success;

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(
                     new IdentityRole<UserId>
                    {
                        Id = new UserId(Guid.NewGuid()),
                        Name = adminRole,
                        NormalizedName = adminRole.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });

            var adminCreationResult = Admin.Create(adminEmail);
            if (adminCreationResult.IsFailed)
                return IdentityResult.Failed(new IdentityError { Description = "Failed to create admin." });

            var userCreationResult = await userManager.CreateAsync(adminCreationResult.Value, adminPassword);
            if (!userCreationResult.Succeeded) 
                return userCreationResult;

            var roleAssignResult = await userManager.AddToRoleAsync(adminCreationResult.Value, "Admin");
            return roleAssignResult;
        }
    }
}

