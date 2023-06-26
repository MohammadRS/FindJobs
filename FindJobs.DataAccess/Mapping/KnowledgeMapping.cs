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
    public class KnowledgeMapping : IEntityTypeConfiguration<Knowledge>
    {
        public void Configure(EntityTypeBuilder<Knowledge> builder)
        {
            builder.ToTable("Knowledges");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .HasMaxLength(220)
                .IsRequired();
           
        }
    }
}
