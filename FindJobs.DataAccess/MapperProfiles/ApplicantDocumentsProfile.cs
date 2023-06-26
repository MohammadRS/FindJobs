using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantDocumentsProfile:Profile
    {
        public ApplicantDocumentsProfile()
        {
            CreateMap<ApplicantDocuments, ApplicantDocumentsDto>().ReverseMap();
        }
    }
}
