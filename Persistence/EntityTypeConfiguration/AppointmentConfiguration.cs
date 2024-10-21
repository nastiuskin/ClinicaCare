﻿using Domain.Appointments;
using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(appointmentId => appointmentId.Value,
                value => new AppointmentId(value));

            builder.Property(a => a.DoctorId)
           .HasConversion(doctorId => doctorId.Value,
                          value => new UserId(value));

            builder.HasOne(a => a.Doctor)
             .WithMany(d => d.Appointments)
             .HasForeignKey(a => a.DoctorId);

            builder.Property(a => a.PatientId)
            .HasConversion(patientId => patientId.Value,
                           value => new UserId(value));

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            builder.Property(a => a.MedicalProcedureId)
            .HasConversion(medicalProcedureId => medicalProcedureId.Value,
                           value => new MedicalProcedureId(value));

            builder.HasOne(a => a.MedicalProcedure)
                .WithMany(mp => mp.Appointments)
                .HasForeignKey(a => a.MedicalProcedureId);

            builder.OwnsOne(a => a.AppointmentDateTime, ts =>
            {
                ts.Property(ts => ts.StartTime)
                .HasColumnName("StartTime")
                .IsRequired();

                ts.Property(ts => ts.EndTime)
                .HasColumnName("EndTime")
                .IsRequired();
            });

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.DoctorFeedback);
        }
    }
}
