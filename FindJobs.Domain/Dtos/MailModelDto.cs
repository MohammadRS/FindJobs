using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class MailModelDto
    {
        public string to { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
