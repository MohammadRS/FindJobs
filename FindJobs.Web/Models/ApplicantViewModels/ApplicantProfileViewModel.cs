using FindJobs.Domain.Dtos;
using System.Collections.Generic;

namespace FindJobs.Web.Models.ApplicantViewModels
{
    public class ApplicantProfileViewModel
    {
        public ApplicantProfileViewModel()
        {
            ApplicantEducationDto = new List<ApplicantEducationDto>();
            ApplicantKnowledgeDtos = new List<ApplicantKnowledgeDto>();
            ApplicantLanguageDto = new List<ApplicantLanguageDto>();
            ApplicantWorkExperienceDto = new List<ApplicantWorkExperienceDto>();
            ApplicantDocuments=new List<ApplicantDocumentsDto>();
        }
        public ApplicantProfileDto ApplicantProfileDto { get; set; }
        public List<ApplicantEducationDto> ApplicantEducationDto { get; set; }
        public List<ApplicantKnowledgeDto> ApplicantKnowledgeDtos { get; set; }
        public List<ApplicantLanguageDto> ApplicantLanguageDto { get; set; }
        public List<ApplicantWorkExperienceDto> ApplicantWorkExperienceDto { get; set; }
        public List<ApplicantDocumentsDto> ApplicantDocuments { get; set; }
    }
}
