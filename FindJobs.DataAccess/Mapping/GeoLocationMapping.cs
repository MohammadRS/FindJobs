using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class GeoLocationMapping:IEntityTypeConfiguration<GeoLocation>
    {
        public void Configure(EntityTypeBuilder<GeoLocation> builder)
        {
            builder.ToTable("GeoLocation");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.City).HasMaxLength(100);
            builder.Property(x => x.CountryCode).HasMaxLength(50);
            builder.Property(x => x.Latitude).HasMaxLength(100);
            builder.Property(x => x.Longitude).HasMaxLength(100);
            builder.Property(x => x.Region).HasMaxLength(100);
            builder.Property(x => x.Timezone).HasMaxLength(100);
            builder.Property(x => x.Zipcode).HasMaxLength(100);
        }
    }
}
