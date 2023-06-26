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
    public class LanguageMapping:IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
