using FindJobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantProfileDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }
       
        public DateTime? DateOfBirth { get; set; }
       
        public DateTime? AvailableDate { get; set; }

        public ReadyToWork ReadyToWorkStatus { get; set; }
        public string Email { get; set; }
        public string ApplicantImage { get; set; }
        public string JobPosition { get; set; }
    }
}
