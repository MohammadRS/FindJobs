using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantLanguageProfile : Profile
    {
        public ApplicantLanguageProfile()
        {
            CreateMap<ApplicantLanguage, ApplicantLanguageDto>().ReverseMap();
        }
    }
}
