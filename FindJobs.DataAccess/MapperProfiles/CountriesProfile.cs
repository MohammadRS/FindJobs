using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class CountriesProfile : Profile
    {
        public CountriesProfile()
        {
            CreateMap<CountryDto, Country>().ReverseMap();
        }
    }
}
