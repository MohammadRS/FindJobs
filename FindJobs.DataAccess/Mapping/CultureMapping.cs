using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class CultureMapping : IEntityTypeConfiguration<Culture>
    {
        public void Configure(EntityTypeBuilder<Culture> builder)
        {
            builder.ToTable("Cultures");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CountryCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(2);

            builder.HasOne(i => i.Countries)
                .WithMany(i => i.Cultures)
                .HasForeignKey(i => i.CountryCode);
        }
    }
}