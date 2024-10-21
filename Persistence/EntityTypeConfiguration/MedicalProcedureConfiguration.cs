using Domain.MedicalProcedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    public class MedicalProcedureConfiguration : IEntityTypeConfiguration<MedicalProcedure>
    {
        public void Configure(EntityTypeBuilder<MedicalProcedure> builder)
        {
            builder.ToTable("Medical Procedures");

            builder.HasKey(mp => mp.MedicalProcedureId);

            builder.Property(mp => mp.MedicalProcedureId)
                .HasConversion(medicalProcedureId => medicalProcedureId.Value,
                value => new MedicalProcedureId(value));

            builder.HasMany(mp => mp.Doctors)
                .WithMany(d => d.MedicalProcedures);
        }
    }
}
