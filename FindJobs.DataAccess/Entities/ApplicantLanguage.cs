using FindJobs.Domain.Enums;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantLanguage : BaseEntity
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public SkillLevel LanguageLevel { get; set; }
        public string ApplicantEmail { get; set; }
        public Applicant Applicant { get; set; }
    }
}
