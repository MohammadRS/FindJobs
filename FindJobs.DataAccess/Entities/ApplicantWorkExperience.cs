using System;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantWorkExperience : BaseEntity
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobPosition { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string ApplicantEmail { get; set; }
        public Applicant Applicant { get; set; }
    }
}
