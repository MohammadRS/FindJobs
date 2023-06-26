using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindJobs.Domain.Dtos
{
    public class VerificationCodeDto:BaseClassDto
    {
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public DateTime CreateVerifyTime { get; set; }
    }
   
}
