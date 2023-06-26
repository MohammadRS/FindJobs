using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindJobs.DataAccess.Entities
{
    public class VerificationCode:BaseEntity
    {
        #region Properties
        public int Id { get; set; }
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public DateTime CreateVerifyTime { get; set; }
        #endregion
    }

}
