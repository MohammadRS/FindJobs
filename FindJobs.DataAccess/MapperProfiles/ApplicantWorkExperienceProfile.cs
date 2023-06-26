using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantWorkExperienceProfile : Profile
    {
        public ApplicantWorkExperienceProfile()
        {
            CreateMap<ApplicantWorkExperience, ApplicantWorkExperienceDto>().ReverseMap();
        }
    }
}
