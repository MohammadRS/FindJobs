using FindJobs.Domain.Enums;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantKnowledgeDto:BaseClassDto
    {
        public KnowledgeLevel KnowledgeLevel { get; set; }
        public string KnowledgeName { get; set; }
        public string ApplicantEmail { get; set; }
        public List<KnowledgeDto> KnowledgeDtos { get; set; }
    }
}
