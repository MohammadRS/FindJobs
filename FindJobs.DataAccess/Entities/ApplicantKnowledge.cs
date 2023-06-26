using FindJobs.Domain.Enums;
using System;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantKnowledge : BaseEntity
    {
        public int Id { get; set; }
        public SkillLevel KnowledgeLevel { get; set; }
        public string KnowledgeName { get; set; }
        public string ApplicantEmail { get; set; }
        public Applicant Applicant { get; set; }
    }
}
