using Domain.Appointments;
using Domain.Doctors;
using Domain.Patients;
using Domain.SeedWork;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
   /* public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
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

            // Doctor's specific fields
            builder.Property("Specialization")
                .HasColumnName("Specialization")
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(d => ((Doctor)d).Specialization)
                 .HasColumnName("Specialization")
                 .IsRequired(false)
                 .HasConversion<int>();

            builder.Property(d => ((Doctor)d).CabinetNumber)
                .HasColumnName("CabinetNumber")
                .IsRequired(false);

            builder.Property(d => ((Doctor)d).Biography)
                .HasColumnName("Biography")
                .IsRequired(false);

            builder.OwnsOne(d => ((Doctor)d).WorkingHours, workHoursBuilder =>
            {
                workHoursBuilder.Property(w => w.StartTime)
                    .HasColumnName("WorkingHours_Start")
                    .IsRequired(false);

                workHoursBuilder.Property(w => w.EndTime)
                    .HasColumnName("WorkingHours_End")
                    .IsRequired(false);
            });

            // Doctor's Appointments
            builder.HasMany(d => ((Doctor)d).Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            // Patient's Appointments
            builder.HasMany(p => ((Patient)p).Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId);
        }
    }*/
}
