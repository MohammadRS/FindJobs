using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Numerics;

namespace FindJobs.DataAccess.Mapping
{
    public class ApplicantOfferRequestMapping : IEntityTypeConfiguration<ApplicantOfferRequest>
    {
        public void Configure(EntityTypeBuilder<ApplicantOfferRequest> builder)
        {
            builder.ToTable("ApplicantOfferRequest");
            builder.HasKey(x => x.Id);

            builder.Property(x=>x.ApplicantEmail).HasMaxLength(200).IsRequired();


        }
    }
}
