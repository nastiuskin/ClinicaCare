using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<UserId>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<UserId>> builder)
        {
            builder.Property(r => r.RoleId)
              .HasConversion(roleId => roleId.Value,
                                 value => new UserId(value));
        }
    }
}
