using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class ApplicantOfferRequestProfile : Profile
    {
        public ApplicantOfferRequestProfile()
        {

            CreateMap<ApplicantOfferRequest, ApplicantOfferRequestDto>()
              .ForMember(x => x.OfferDocumentsDto, y => y.MapFrom(z => z.OfferDocuments))
              .ReverseMap();

        }
    }

}
