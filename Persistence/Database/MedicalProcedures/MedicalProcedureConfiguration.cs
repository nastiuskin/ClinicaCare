using Domain.MedicalProcedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.MedicalProcedures
{
    public class MedicalProcedureConfiguration : IEntityTypeConfiguration<MedicalProcedure>
    {
        public void Configure(EntityTypeBuilder<MedicalProcedure> builder)
        {
            builder.ToTable("MedicalProcedures");

            builder.HasKey(mp => mp.Id);

            builder.Property(mp => mp.Id)
                .HasConversion(
                     medicalProcedureId => medicalProcedureId.Value,
                     value => new MedicalProcedureId(value))
                 .IsRequired();

            builder.HasMany(mp => mp.Doctors)
                .WithMany(d => d.MedicalProcedures);
        }
    }
}
