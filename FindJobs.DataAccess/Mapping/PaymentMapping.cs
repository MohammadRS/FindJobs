using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GetwayType).HasMaxLength(15);
            builder.Property(x=>x.Currency).HasMaxLength(3);

        }
    }
}
