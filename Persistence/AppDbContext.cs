using Domain.Appointments;
using Domain.Doctors;
using Domain.MedicalProcedures;
using Domain.Patients;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;


namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<MedicalProcedure> MedicalProcedures { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


    }

}
