using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    internal class ApplicantOffersFavouriteProfile:Profile
    {
        public ApplicantOffersFavouriteProfile()
        {
           CreateMap<ApplicantOffersFavourite,ApplicantOffersFavouriteDto>().ReverseMap();
        }
    }
}
