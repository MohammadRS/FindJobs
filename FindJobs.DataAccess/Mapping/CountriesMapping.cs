using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class CountriesMapping : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");
            builder.HasKey(x => x.Code);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Code).HasMaxLength(10);
            builder.Property(x => x.CurrencyCode).HasMaxLength(10);
            builder.Property(x => x.CurrencyName).HasMaxLength(200);
            builder.Property(x => x.ContinentCode).HasMaxLength(10);
            builder.Property(x => x.PhonePrefix).HasMaxLength(200);
            builder.Property(x => x.Capital).HasMaxLength(200);
            builder.Property(x => x.CurrencySign).HasMaxLength(200);

          
        }
    }
}