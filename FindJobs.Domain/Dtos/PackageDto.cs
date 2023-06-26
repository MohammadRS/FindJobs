using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class PackageDto : BaseClassDto
    {
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public int DurationInDays { get; set; }
    }
}
