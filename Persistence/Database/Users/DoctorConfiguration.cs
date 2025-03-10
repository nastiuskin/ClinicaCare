﻿using Domain.Users.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Database.Users
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.OwnsOne(d => d.WorkingHours, wh =>
            {
                wh.Property(wh => wh.StartTime)
                    .HasColumnName("WorkingStartTime")
                    .IsRequired();

                wh.Property(wh => wh.EndTime)
                    .HasColumnName("WorkingEndTime")
                    .IsRequired();
            });
        }
    }
}
