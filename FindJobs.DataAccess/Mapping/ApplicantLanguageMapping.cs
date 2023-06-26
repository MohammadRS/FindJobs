using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantLanguageMapping : IEntityTypeConfiguration<ApplicantLanguage>
    {
        public void Configure(EntityTypeBuilder<ApplicantLanguage> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.LanguageLevel)
                .HasMaxLength(40)
                .IsRequired();
            builder.Property(a => a.LanguageName)
            .HasMaxLength(60)
            .IsRequired();
            builder.HasOne(a => a.Applicant)
                .WithMany(a => a.ApplicantLanguages)
                .HasForeignKey(a => a.ApplicantEmail).IsRequired();
        }
    }
}
