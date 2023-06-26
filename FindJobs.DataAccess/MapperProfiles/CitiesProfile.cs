using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class CitiesProfile : Profile
    {
        public CitiesProfile()
        {
            CreateMap<CityDto, City>().ReverseMap();
              
                
        }
    }
}
