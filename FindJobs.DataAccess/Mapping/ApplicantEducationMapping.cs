using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantEducationMapping : IEntityTypeConfiguration<ApplicantEducation>
    {
        public void Configure(EntityTypeBuilder<ApplicantEducation> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CourseName)
                .HasMaxLength(120)
                .IsRequired();
            builder.Property(a => a.InstituteName)
                .HasMaxLength(160)
                .IsRequired();
            builder.HasOne(a => a.Applicant)
               .WithMany(a => a.ApplicantEducations)
                .HasForeignKey(a => a.ApplicantEmail).IsRequired();
        }
    }
}
