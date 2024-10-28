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

            builder.Property(mp => mp.Name)
                .IsRequired();

            builder.HasIndex(mp => mp.Name)
                .IsUnique(true);

            builder.Property(mp => mp.Price)
               .IsRequired();

            builder.Property(mp => mp.Type)
               .IsRequired();

            builder.Property(mp => mp.Id)
                .HasConversion(
                     medicalProcedureId => medicalProcedureId.Value,
                     value => new MedicalProcedureId(value));

            builder.HasMany(mp => mp.Doctors)
                .WithMany(d => d.MedicalProcedures);
        }
    }
}
