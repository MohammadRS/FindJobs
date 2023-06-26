using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<Applicant, ApplicantDto>()
                .ForMember(x=>x.ApplicantImageValue, y=>y.MapFrom(z=>z.ApplicantImage))
                .ReverseMap();
            CreateMap<Applicant, ApplicantProfileDto>().ReverseMap();
            CreateMap<Applicant, ApplicantContactDetailsDto>().ReverseMap();
            CreateMap<Applicant, ApplicantAddtionalSectionDto>().ReverseMap();
          

        }
    }
}
