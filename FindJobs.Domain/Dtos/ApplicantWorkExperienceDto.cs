using System;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantWorkExperienceDto:BaseClassDto
    {
        public string JobTitle { get; set; }
        public string JobPosition { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string ApplicantEmail { get; set; }
       
    }
}
