using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class VerificationCodeMapping : IEntityTypeConfiguration<VerificationCode>
    {
        public void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.ToTable("VerificationCodes");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.VerifyCode).HasMaxLength(6).IsRequired();
            builder.Property(x=>x.CreateVerifyTime).HasMaxLength(50).IsRequired();
           

         

        }
    }
}
