using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantKnowledgeMapping : IEntityTypeConfiguration<ApplicantKnowledge>
    {
        public void Configure(EntityTypeBuilder<ApplicantKnowledge> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.KnowledgeName)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasOne(a => a.Applicant)
                .WithMany(a => a.ApplicantKnowledges)
                .HasForeignKey(a=>a.ApplicantEmail).IsRequired();
        }
    }
}
