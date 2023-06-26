using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantMapping : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.ToTable("Applicants");
            builder.HasKey(x => x.Email);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.PostalCode).HasMaxLength(12);
            builder.Property(x => x.Address).HasMaxLength(95);
            builder.Property(x => x.HourlyAverage).HasMaxLength(10);
            builder.Property(x => x.HourlyAverage).HasMaxLength(10);
            builder.Property(x => x.JobPosition).HasMaxLength(50);

            builder.HasOne(a => a.Country)
                .WithMany(a => a.Applicants)
                .HasForeignKey(a => a.CountryCode);
        }
    }
}
