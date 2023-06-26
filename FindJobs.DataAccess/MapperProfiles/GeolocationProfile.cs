using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class GeolocationProfile : Profile
    {
        public GeolocationProfile()
        {
            CreateMap<GeoLocationDto, GeoLocation>().ReverseMap();
        }
    }
}
