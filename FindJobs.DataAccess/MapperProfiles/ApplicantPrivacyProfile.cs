using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using AutoMapper;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantPrivacyProfile : Profile
    {
        public ApplicantPrivacyProfile()
        {
            CreateMap <Applicant, ApplicantPrivacyDto>().ReverseMap();
        }

    }
}
