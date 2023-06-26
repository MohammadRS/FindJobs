using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class ClaimRoleDto
    {
        public bool IsApplicant { get; set; }
        public bool IsCompany { get; set; }
    }
}
