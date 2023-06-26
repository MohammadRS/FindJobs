using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Entities
{
    public class Package:BaseEntity
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public int DurationInDays { get; set; }


    }
}
