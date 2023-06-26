using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Entities
{
    public class Currency:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal CurrencyRate { get; set; }
    }
}
