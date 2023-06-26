using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class PackageMapping : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Packages");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.PackageName)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}
