using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class OfferDocumentsProfile : Profile
    {
        public OfferDocumentsProfile()
        {
            CreateMap<OfferDocuments, OfferDocumentsDto>().ReverseMap();

        }
    }

}
