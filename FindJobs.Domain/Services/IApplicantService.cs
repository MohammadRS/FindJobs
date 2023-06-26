using FindJobs.Domain.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IApplicantService
    {
        #region Claims
        bool GetApplicantUserClaims(ClaimsPrincipal user);
        #endregion
        #region Applicants
        ApplicantDto GetApplicant(string applicantEmail);
        ApplicantDto GetApplicant(int id);
        PaginationDto<ApplicantDto> GetApplicants(int currentPage, string key, string location);
        int GetApplicantsCount(string key, string location);
        #endregion
        #region Applicant Privacy
        Task<bool> UpdatePrivacy(ApplicantPrivacyDto model);
        #endregion
        #region Applicant Image
        Task<bool> UpdateApplicantImage(string imageBase64, string applicantEmail);
        Task<bool> IsApplicantExist(string email);
        #endregion
        #region ApplicantDocuments
        List<ApplicantDocumentsDto> GetApplicantDocumentsByEmail(string email);
        Task<bool> InsertApplicantDocument(ApplicantDocumentsDto applicantDocumentsDto);
        Task<bool> UpdateApplicantDocument(ApplicantDocumentsDto applicantDocumentsDto);
        Task<bool> DeleteApplicantDocument(int id);
        #endregion
        #region WorkExperience
        Task<bool> InsertOrUpdateWorkExperiance(ApplicantWorkExperienceDto workExperienceDto);
        List<ApplicantWorkExperienceDto> GetAllApplicantWorkExperienceByEmail(string email);
        Task<ApplicantWorkExperienceDto> GetWorkExperienceById(int id);
        Task<bool> DeleteApplicantWorkExperience(int id);
        #endregion
        #region ApplicantEducation
        Task<bool> CreateOrUpdateEducation(ApplicantEducationDto educationDto);
        List<ApplicantEducationDto> GetAllApplicantEducation(string email);
        Task<ApplicantEducationDto> GetApplicantEducationById(int id);
        Task<bool> DeleteApplicantEducation(int id);
        #endregion
        #region knowledge
        Task<bool> CreateOrUpdateKhnowledge(ApplicantKnowledgeDto knowledgeDto);
        List<ApplicantKnowledgeDto> GetAllApplicantKnowledge(string email);
        Task<ApplicantKnowledgeDto> GetApplicantKnowledgeById(int id);
        Task<bool> DeleteApplicantKnowledge(int id);
        List<KnowledgeDto> GetKnowledgeList();
        #endregion
        #region Language
        Task<bool> CreateOrUpdateLanguage(ApplicantLanguageDto languageDto);
        List<ApplicantLanguageDto> GetAllApplicantLanguage(string email);
        Task<ApplicantLanguageDto> GetApplicantLanguageById(int id);
        Task<bool> DeleteApplicantLanguage(int id);
        List<LanguageDto> GetLanguageList();

        #endregion
        #region Personal Information
        ApplicantDto GetPersonalInformation(string email);
        Task<bool> SavePersonalInformation(ApplicantProfileDto personalInformation);
        #endregion
        #region ContactDetail
        ApplicantDto GetApplicantContactDetail(string email);
        Task<bool> SaveApplicantContactDetail(ApplicantContactDetailsDto applicantContactDetailsDto, string email);
        #endregion
        #region Additional Section
        ApplicantDto GetApplicantAdditionalSection(string email);
        Task<bool> SaveApplicantAdditionalSection(ApplicantAddtionalSectionDto addtionalSectionDto);
        #endregion
        #region Email Preferences
        Task<bool> UpdateEmailPreferences(ApplicantPreferenceDto model);
        bool UnsubscribeEmailPreferences(string email);
        List<ApplicantPreferenceDto> GetEmailPreferences(string email);
        #endregion
        #region Offers
        List<OfferDto> GetOffers();
        PaginationDto<OfferDto> SearchOffersAjax(string location, int currentPage, string language, string workPlace,
            string jobOffer, string employer, string position, string company, List<string> jobCategories = null);
        PaginationDto<OfferDto> SearchOffersAjax();
        Task<ApplicantOffersFavouriteResult> SaveApplicantOfferFavourite(ApplicantOffersFavouriteDto applicantOffersFavouriteDto);
        List<OfferDto> GetAllFavourteApplicantOffers(string applicantEmail);
        Task<bool> DeleteFavouriteOffer(int id, string applicantEmail);
        int GetCountFavouritApplicant(string applicantEmail);
        List<ApplicantOffersFavouriteDto> applicantOffersFavouriteDtos(string applicantEmail);
        #endregion

    }
}
