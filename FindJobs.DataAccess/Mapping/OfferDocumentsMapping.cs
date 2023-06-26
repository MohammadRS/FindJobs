using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class OfferDocumentsMapping : IEntityTypeConfiguration<OfferDocuments>
    {
        public void Configure(EntityTypeBuilder<OfferDocuments> builder)
        {
            builder.ToTable("OfferDocuments");
            builder.HasKey(x => x.Id);
        }
    }
}
