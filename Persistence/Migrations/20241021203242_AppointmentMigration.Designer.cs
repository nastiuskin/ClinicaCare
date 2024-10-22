﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241021203242_AppointmentMigration")]
    partial class AppointmentMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DoctorMedicalProcedure", b =>
                {
                    b.Property<Guid>("DoctorsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MedicalProceduresId")
                        .HasColumnType("uuid");

                    b.HasKey("DoctorsId", "MedicalProceduresId");

                    b.HasIndex("MedicalProceduresId");

                    b.ToTable("DoctorMedicalProcedure");
                });

            modelBuilder.Entity("Domain.Appointments.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("DoctorFeedback")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MedicalProcedureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("MedicalProcedureId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments", (string)null);
                });

            modelBuilder.Entity("Domain.MedicalProcedures.MedicalProcedure", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MedicalProcedures", (string)null);
                });

            modelBuilder.Entity("Domain.SeedWork.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("Admin");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Doctors.Doctor", b =>
                {
                    b.HasBaseType("Domain.SeedWork.User");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CabinetNumber")
                        .HasColumnType("integer");

                    b.Property<int>("Specialization")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Doctor");
                });

            modelBuilder.Entity("Domain.Patients.Patient", b =>
                {
                    b.HasBaseType("Domain.SeedWork.User");

                    b.HasDiscriminator().HasValue("Patient");
                });

            modelBuilder.Entity("DoctorMedicalProcedure", b =>
                {
                    b.HasOne("Domain.Doctors.Doctor", null)
                        .WithMany()
                        .HasForeignKey("DoctorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.MedicalProcedures.MedicalProcedure", null)
                        .WithMany()
                        .HasForeignKey("MedicalProceduresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Appointments.Appointment", b =>
                {
                    b.HasOne("Domain.Doctors.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.MedicalProcedures.MedicalProcedure", "MedicalProcedure")
                        .WithMany("Appointments")
                        .HasForeignKey("MedicalProcedureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Patients.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.ValueObjects.TimeSlot", "AppointmentDateTime", b1 =>
                        {
                            b1.Property<Guid>("AppointmentId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("EndTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndTime");

                            b1.Property<DateTime>("StartTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartTime");

                            b1.HasKey("AppointmentId");

                            b1.ToTable("Appointments");

                            b1.WithOwner()
                                .HasForeignKey("AppointmentId");
                        });

                    b.Navigation("AppointmentDateTime")
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("MedicalProcedure");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Domain.SeedWork.User", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Domain.ValueObjects.PhoneNumber", "PhoneNumber", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("PhoneNumber");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("PhoneNumber")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Doctors.Doctor", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.TimeSlot", "WorkingHours", b1 =>
                        {
                            b1.Property<Guid>("DoctorId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("EndTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("WorkingEndTime");

                            b1.Property<DateTime>("StartTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("WorkingStartTime");

                            b1.HasKey("DoctorId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("DoctorId");
                        });

                    b.Navigation("WorkingHours")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.MedicalProcedures.MedicalProcedure", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Domain.Doctors.Doctor", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Domain.Patients.Patient", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
