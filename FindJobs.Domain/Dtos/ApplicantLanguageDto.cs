using FindJobs.Domain.Enums;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantLanguageDto:BaseClassDto
    {
        public string LanguageName { get; set; }
        public SkillLevel LanguageLevel { get; set; }
        public string ApplicantEmail { get; set; }
        public List<LanguageDto> languageDtos { get; set; }
    }
}
