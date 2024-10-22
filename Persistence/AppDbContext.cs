using Domain.Appointments;
using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Configuration;
using Persistence.EntityTypeConfiguration;


namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MedicalProcedure> MedicalProcedures { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MedicalProcedureConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
        }


    }

}
