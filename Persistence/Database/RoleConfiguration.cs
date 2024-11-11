using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<UserId>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<UserId>> builder)
        {
            builder.Property(r => r.Id)
                    .HasConversion(roleId => roleId.Value,
                                     value => new UserId(value));
        }
    }
}
