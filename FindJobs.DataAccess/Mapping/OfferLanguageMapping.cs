using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindJobs.DataAccess.Mapping
{
    public class OfferLanguageMapping : IEntityTypeConfiguration<OfferLanguage>
    {
        public void Configure(EntityTypeBuilder<OfferLanguage> builder)
        {
            builder.ToTable("OfferLanguages");
        }
    }
}
