using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth
{
    public class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole<UserId>> roleManager)
        {
            var roles = new List<string> { "Admin", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    var identityRole = new IdentityRole<UserId>
                    {
                        Id = new UserId(Guid.NewGuid()),
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };

                    await roleManager.CreateAsync(identityRole);
                }
            }
        }
    }
}