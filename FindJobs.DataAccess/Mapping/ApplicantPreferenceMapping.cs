using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicationPreferenceMapping : IEntityTypeConfiguration<ApplicantPreference>
    {
        public void Configure(EntityTypeBuilder<ApplicantPreference> builder)
        {
            builder.ToTable("ApplicantPreference");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Category).HasMaxLength(100);

            builder.HasOne(x => x.Applicant).WithMany(x => x.ApplicantPreferences)
                .HasForeignKey(x => x.ApplicantEmail);
        }
    }
}