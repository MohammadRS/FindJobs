using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class CitiesMapping : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.AsciiName).HasMaxLength(200);
            builder.Property(x => x.Iso3).HasMaxLength(100);
            builder.Property(x => x.StateName).HasMaxLength(200);
            builder.Property(x => x.Capital).HasMaxLength(200);
            builder.Property(x => x.Lat).HasMaxLength(100);
            builder.Property(x => x.Lng).HasMaxLength(100);
            builder.Property(x => x.Population).HasMaxLength(300);

           
           
                
        }
    }
}