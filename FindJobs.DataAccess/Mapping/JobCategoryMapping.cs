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
    public class JobCategoryMapping : IEntityTypeConfiguration<JobCategory>
    {
        public void Configure(EntityTypeBuilder<JobCategory> builder)
        {
            builder.ToTable("JobCategories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Jobcategory).HasMaxLength(100);
        }
    }
}
