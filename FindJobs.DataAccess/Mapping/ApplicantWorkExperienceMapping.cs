using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantWorkExperienceMapping : IEntityTypeConfiguration<ApplicantWorkExperience>
    {
        public void Configure(EntityTypeBuilder<ApplicantWorkExperience> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.JobTitle)
                .HasMaxLength(80)
                .IsRequired();
            builder.Property(a => a.JobPosition)
                .HasMaxLength(100);
            builder.HasOne(a => a.Applicant)
                .WithMany(a => a.ApplicantWorkExperiences)
                .HasForeignKey(a => a.ApplicantEmail).IsRequired();
        }
    }
}
