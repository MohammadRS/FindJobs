using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantPreferenceProfile : Profile
    {
        public ApplicantPreferenceProfile()
        {
            CreateMap<ApplicantPreferenceDto, ApplicantPreference>().ReverseMap();
        }
    }
}