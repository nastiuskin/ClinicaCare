﻿using Domain.Appointments;
using Domain.SeedWork;
using Domain.Users;
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
                .HasValue<User>("Admin")
                .HasValue<Doctor>("Doctor")
                .HasValue<Patient>("Patient");

            builder.Property(u => u.FirstName)
                .IsRequired();

            builder.Property(u => u.LastName)
                .IsRequired();

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.OwnsOne(u => u.PhoneNumber, phoneBuilder =>
            {
                phoneBuilder.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired();
            });

            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.Property(p => p.Value)
                    .HasColumnName("Email")
                    .IsRequired();
            });

        }
    }
}
