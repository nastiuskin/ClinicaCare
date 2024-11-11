using Domain.Users;
using Domain.Users.Admins;
using Domain.Users.Doctors;
using Domain.Users.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(userId => userId.Value,
                               value => new UserId(value));

            builder.HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin")
                .HasValue<Doctor>("Doctor")
                .HasValue<Patient>("Patient");

            builder.Property(u => u.FirstName)
                .IsRequired(false);

            builder.Property(u => u.LastName)
                .IsRequired(false);

            builder.Property(u => u.DateOfBirth);

            builder.Property(u => u.RefreshTokenExpiryTime);
        }
    }
}
