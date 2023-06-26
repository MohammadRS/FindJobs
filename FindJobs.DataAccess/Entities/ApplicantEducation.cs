using FindJobs.Domain.Enums;
using System;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantEducation : BaseEntity
    {
        public int Id { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public string CourseName { get; set; }
        public string InstituteName { get; set; }
        public DateTime? StartEducation { get; set; }
        public DateTime? EndEducation { get; set; }
        public string ApplicantEmail { get; set; }
        public Applicant Applicant { get; set; }
    }
}
