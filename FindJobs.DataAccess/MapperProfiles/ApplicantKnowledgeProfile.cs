using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantKnowledgeProfile:Profile
    {
        public ApplicantKnowledgeProfile()
        {
            CreateMap<ApplicantKnowledge, ApplicantKnowledgeDto>().ReverseMap();
        }
    }
}
