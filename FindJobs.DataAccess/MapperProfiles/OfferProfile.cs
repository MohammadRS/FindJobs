using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Routing.Constraints;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.OfferJobCategoryDtos, src => src.MapFrom(x => x.OfferJobCategories))
                .ForMember(dest => dest.CompanyDto, src => src.MapFrom(x => x.Company))
                .ForMember(dest => dest.OfferLanguageDtos, src => src.MapFrom(x => x.OfferLanguages))
                .ForMember(dest => dest.OfferKnowledgeDtos, src => src.MapFrom(x => x.OfferKnowledges))
                .ForMember(dest=>dest.applicantOffersFavouriteDtos,src=>src.MapFrom(x=>x.ApplicantOffersFavourites))
                .ReverseMap();

            CreateMap<OfferJobCategory, OfferJobCategoryDto>()
                .ForMember(dest => dest.JobCategoryDto, src => src.MapFrom(x => x.JobCategory))
                .ReverseMap();

            CreateMap<OfferLanguage, OfferLanguageDto>()
                .ForMember(dest => dest.LanguageDto, src => src.MapFrom(x => x.Language))
                .ReverseMap();

            CreateMap<OfferKnowledge, OfferKnowledgeDto>()
                 .ForMember(dest => dest.KnowledgeDto, src => src.MapFrom(x => x.Knowledge))
                .ReverseMap();
        }

    }
}
