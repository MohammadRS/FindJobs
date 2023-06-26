using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantDocumentsMapping : IEntityTypeConfiguration<ApplicantDocuments>
    {
        public void Configure(EntityTypeBuilder<ApplicantDocuments> builder)
        {
            builder.ToTable("ApplicantDocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExtensionFile).HasMaxLength(20);
            builder.Property(x => x.ApplicantEmail).IsRequired();
            builder.HasOne(x => x.Applicant).WithMany(x => x.ApplicantDocuments)
                    .HasForeignKey(x => x.ApplicantEmail);
        }
    }
}
