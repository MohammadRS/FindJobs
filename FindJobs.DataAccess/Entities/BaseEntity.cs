using System;

namespace FindJobs.DataAccess.Entities
{
    public class BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
