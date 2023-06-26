using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class CulturesProfile : Profile
    {
        public CulturesProfile()
        {
            CreateMap<CultureDto, Culture>().ReverseMap();
            CreateMap<SeedDataCultureDto, Culture>().ReverseMap();
        }
    }
}
