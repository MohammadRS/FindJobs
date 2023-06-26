using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class EmailModelDto
    {
        public string Email { get; set; }
        public string VerifyCode { get; set; }
    }
}
