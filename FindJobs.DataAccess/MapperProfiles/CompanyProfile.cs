using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
               .ForMember(dest=>dest.CityDto,src=>src.MapFrom(x=>x.City))
                .ReverseMap();
        }
    }
}
