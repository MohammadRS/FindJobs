using FindJobs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Mapping
{
    public class OfferKnowledgeMapping : IEntityTypeConfiguration<OfferKnowledge>
    {
        public void Configure(EntityTypeBuilder<OfferKnowledge> builder)
        {
            builder.ToTable("OfferKnowledges");
        }
    }
}
