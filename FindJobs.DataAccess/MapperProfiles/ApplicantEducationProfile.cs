using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantEducationProfile : Profile
    {
        public ApplicantEducationProfile()
        {
            CreateMap< ApplicantEducation,ApplicantEducationDto>().ReverseMap();
        }
    }
}
