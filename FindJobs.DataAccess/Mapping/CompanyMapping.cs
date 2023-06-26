using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class CompanyMapping : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");
            builder.HasKey(x => x.Email);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.ContactPersonEmail).HasMaxLength(200);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.ContactPersonPhone).HasMaxLength(20);
            builder.Property(x => x.Street).HasMaxLength(100);
            builder.Property(x => x.PostalCode).HasMaxLength(20);
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.Property(x => x.NumberOfEmployee).HasMaxLength(50);
            builder.Property(x => x.AboutCompany).HasMaxLength(1000);
        }
    }
}
