using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class VerificationCodeProfile:Profile
    {
        public VerificationCodeProfile()
        {
            CreateMap<VerificationCodeDto, VerificationCode>().ReverseMap();
        }
    }
}
