using FindJobs.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantEducationDto:BaseClassDto
    {
        public EducationLevel EducationLevel { get; set; }
        public string CourseName { get; set; }
        public string InstituteName { get; set; }
        public DateTime? StartEducation { get; set; }
        public DateTime? EndEducation { get; set; }
        public string ApplicantEmail { get; set; }
    }
}
