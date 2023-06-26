using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using FindJobs.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : Controller
    {
        private readonly IApplicantService applicantService;
        private readonly IEncryption encryption;
        private readonly IConfiguration configurtion;
        private readonly IRazorPartialToStringService renderer;

        public ApplicantController(IApplicantService applicantService,
            IEncryption encryption,
            IConfiguration configurtion,
            IRazorPartialToStringService renderer)
        {
            this.applicantService = applicantService;
            this.encryption = encryption;
            this.configurtion = configurtion;
            this.renderer = renderer;
        }
        #region Applicant
        [NonAction]
        private string GetApplicantEmail()
        {
            var claims = User.Claims.ToList();
            var applicantEmail = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return applicantEmail;
        }

        [HttpGet("GetCurrentApplicant")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ResultDto<ApplicantDto>> GetCurrentApplicant()
        {
            var model = applicantService.GetApplicant(GetApplicantEmail());
            var allCountries = await Domain.Global.Global.GetCountryList(configurtion["GlobalSettings:ApiUrl"]);
            var allCities = await Domain.Global.Global.GetCityList(configurtion["GlobalSettings:ApiUrl"]);

            if (model.CountryCode != null)
                model.CountryName = allCountries.SingleOrDefault(x => x.Code == model.CountryCode).Name;

            if (model.CityId != null)
                model.CityMainName = allCities.SingleOrDefault(x => x.Id == model.CityId).Name;
            return new ResultDto<ApplicantDto>(model);
        }

        private ApplicantDto AffectApplicantPrivacy(ApplicantDto applicant)
        {
            applicant.Email = "";
            if (applicant.ShowGender) applicant.Gender = null;
            if (applicant.ShowPhone) applicant.Phone = null;
            if (applicant.ShowAddress) applicant.Address = null;
            if (applicant.ShowAge) applicant.DateOfBirth = null;
            if (applicant.ShowCountryOrCity) applicant.City = null;
            if (applicant.ShowCountryOrCity) applicant.Country = null;
            return applicant;
        }
        [HttpGet("GetApplicantById")]
        public IActionResult GetApplicantById(int id)
        {
            //IMPORTANT NOTE: (DO NOT DELETE THIS COMMENT)
            //WE USE THIS METHOD TO RETURN APPLICANT DATA FOR PUBLIC (NOT FOR HIMSELF/HERSELF)
            //IT MEANS WE SHOULD NOT HAVE PRIVATE FIELDS IN THIS METHOD
            var applicant = applicantService.GetApplicant(id);
            if (applicant != null)
            {
                AffectApplicantPrivacy(applicant);
                return new JsonResult(new ResultDto<ApplicantDto>(applicant));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));


        }
        [HttpGet("GetApplicants")]
        public ResultDto<PaginationDto<ApplicantDto>> GetApplicants(string? key, string? location, int currentPage = 1)
        {
            //IMPORTANT NOTE: (DO NOT DELETE THIS COMMENT)
            //WE USE THIS METHOD TO RETURN APPLICANT DATA FOR PUBLIC (NOT FOR HIMSELF/HERSELF)
            //IT MEANS WE SHOULD NOT HAVE PRIVATE FIELDS IN THIS METHOD
            var applicants = new ResultDto<PaginationDto<ApplicantDto>>(applicantService.GetApplicants(currentPage, key, location));
            if (applicants.Data.ItemsCount > 0)
            {
                foreach (var applicant in applicants.Data.Data)
                {
                    AffectApplicantPrivacy(applicant);
                }
            }
            return applicants;
        }
        #endregion
        [HttpPost("UpdatePrivacy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePrivacy(ApplicantPrivacyDto privacyDto)
        {

            var applicantEmail = GetApplicantEmail();

            var applicantPrivacyDto = new ApplicantPrivacyDto
            {
                Email = applicantEmail,
                AllowSearchEngines = privacyDto.AllowSearchEngines,
                ProfileVisible = privacyDto.ProfileVisible,
                ShowAddress = privacyDto.ShowAddress,
                ShowAge = privacyDto.ShowAge,
                ShowCountryOrCity = privacyDto.ShowCountryOrCity,
                ShowGender = privacyDto.ShowGender,
                ShowPhone = privacyDto.ShowPhone
            };
            await applicantService.UpdatePrivacy(applicantPrivacyDto);

            return new JsonResult(new { MessageCodes.Success });
        }
        [HttpPost("UpdateEmailPreferences")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateEmailPreferences(EmailPreferenceViewModel emailPreference)
        {

            var applicantEmail = GetApplicantEmail();
            var applicantPreferenceDto = new ApplicantPreferenceDto
            { ApplicantEmail = applicantEmail, IsSubscribed = false };
            var categories = emailPreference.GetType()
                .GetProperties()
                .Select(p =>
                {
                    var value = p.Name + "," + p.GetValue(emailPreference, null);
                    return value?.ToString();
                }).ToArray();
            foreach (var category in categories.Where(x => !x.Equals("ApplicantEmail,")))
            {
                var item = category.Split(",");
                applicantPreferenceDto.Category = item[0];
                applicantPreferenceDto.IsSubscribed = !string.IsNullOrWhiteSpace(item[1]);
                await applicantService.UpdateEmailPreferences(applicantPreferenceDto);
            }

            return new JsonResult(new { MessageCodes.Success });
        }
        [HttpGet("GetApplicantImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicantImage()
        {
            var applicant = applicantService.GetApplicant(GetApplicantEmail());
            if (applicant != null)
            {
                return new JsonResult(new ResultDto<string>(applicant.ApplicantImage));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }
        [HttpGet("IsApplicantExist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> IsApplicantExist()
        {
            bool available = await applicantService.IsApplicantExist(GetApplicantEmail());
            if (available)
            {
                return new JsonResult(new ResultDto<bool>(true));
            }
            return new JsonResult(new ResultDto<bool>(false));
        }

        [HttpGet("UnsubscribeEmailPreferences/{EncryptedEmail}")]
        public IActionResult UnsubscribeEmailPreferences(string EncryptedEmail)
        {
            var applicantEmail = encryption.Decrypt(HttpUtility.UrlDecode(EncryptedEmail), configurtion["GlobalSettings:EncryptionSalt"]);
            if (applicantService.UnsubscribeEmailPreferences(applicantEmail))
            {
                return new JsonResult(new ResultDto((int)MessageCodes.Success));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }

        [HttpPost("UpdateApplicantImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateApplicantImage([FromBody] string imageBase64)
        {
            var applicantEmail = GetApplicantEmail();
            var result = await applicantService.UpdateApplicantImage(imageBase64, applicantEmail);
            if (result)
                return new JsonResult(new ResultDto((int)MessageCodes.Success));
            else
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }
        [HttpGet("CalculationProgressbar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CalculationProgressbar()
        {
            var progressbar = new CalculateProgressbarDto();
            var applicantdto = applicantService.GetApplicant(GetApplicantEmail());
            int profileCompleted = 15;

            if (applicantdto.FirstName != null) profileCompleted += 5;
            if (applicantdto.LastName != null) profileCompleted += 5;
            if (applicantdto.DateOfBirth != null) profileCompleted += 5;
            if (applicantdto.AvailableDate != null) profileCompleted += 5;

            if (applicantdto.Phone != null) profileCompleted += 5;
            if (applicantdto.CountryCode != null) profileCompleted += 5;
            if (applicantdto.Address != null) profileCompleted += 5;
            if (applicantdto.PostalCode != null) profileCompleted += 5;

            if (applicantdto.ApplicantDocuments.Count > 0) profileCompleted += 5;
            if (applicantdto.ApplicantWorkExperiences.Count > 0) profileCompleted += 35;
            if (applicantdto.ApplicantEducations.Count > 0) profileCompleted += 5;
            decimal profileProgressBarOffset = 450 - 450 * (Convert.ToDecimal(profileCompleted) / 100);
            progressbar.ProfileCompleted = profileCompleted;
            progressbar.ProfileProgressBarOffset = profileProgressBarOffset;
            return new JsonResult(new ResultDto<CalculateProgressbarDto>(progressbar));
        }

        [HttpGet("CheckApplicantRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetRoleClaims()
        {
            //TODO: we have a security issue. check this scenario:
            //Sign up as both applicant and company with the same email
            //login with a company and copy the token, then in swagger call UpdatePrivacy for applicant
            if (applicantService.GetApplicantUserClaims(User))
                return new JsonResult(new ResultDto((int)MessageCodes.Success));
            else
                return new JsonResult(new ResultDto((int)MessageCodes.Unauthorized));
        }

        #region ApplicantDocument
        [HttpPost("InsertApplicantDocument")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertApplicantDocument(ApplicantDocumentsDto applicantDocumentsDto)
        {
            var email = GetApplicantEmail();
            applicantDocumentsDto.ApplicantEmail = email;
            if (!await applicantService.InsertApplicantDocument(applicantDocumentsDto))
            {
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            }
            return GetApplicantDocumentsByEmail();
        }
        [HttpGet("GetApplicantDocumentsByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicantDocumentsByEmail()
        {
            var email = GetApplicantEmail();
            List<ApplicantDocumentsDto> model = applicantService.GetApplicantDocumentsByEmail(email);

            return new JsonResult(new ResultDto<List<ApplicantDocumentsDto>>(model));
        }

        [HttpGet("DeleteApplicantDocument")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteApplicantDocument(int id)
        {
            var email = GetApplicantEmail();
            if (!await applicantService.DeleteApplicantDocument(id))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicantDocumentsByEmail();
        }
        #endregion
        #region WorkExperience

        [HttpPost("InsertOrUpdateWorkExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertOrUpdateWorkExperience(ApplicantWorkExperienceDto workExperienceDto)
        {
            workExperienceDto.ApplicantEmail = GetApplicantEmail();
            if (!await applicantService.InsertOrUpdateWorkExperiance(workExperienceDto))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicantWorkExperienceByEmail();
        }
        [HttpGet("GetWorkExperienceById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetWorkExperienceById(int id)
        {
            var workExperienceDto = await applicantService.GetWorkExperienceById(id);
            if (workExperienceDto == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return new JsonResult(new ResultDto<ApplicantWorkExperienceDto>(workExperienceDto));
        }
        [HttpGet("DeleteWorkExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteWorkExperience(int id)
        {
            if (!await applicantService.DeleteApplicantWorkExperience(id))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));


            return GetApplicantWorkExperienceByEmail();
        }

        [HttpGet("GetApplicantWorkExperienceByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicantWorkExperienceByEmail()
        {
            var email = GetApplicantEmail();
            List<ApplicantWorkExperienceDto> model = applicantService.GetAllApplicantWorkExperienceByEmail(email);
            return new JsonResult(new ResultDto<List<ApplicantWorkExperienceDto>>(model));
        }

        #endregion
        #region ApplicantEducation
        [HttpPost("CreateOrUpdateApplicantEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateOrUpdateApplicantEducation(ApplicantEducationDto applicantEducationDto)
        {
            applicantEducationDto.ApplicantEmail = GetApplicantEmail();
            if (!await applicantService.CreateOrUpdateEducation(applicantEducationDto))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicanEducationListByEmail();
        }
        [HttpGet("GetApplicantEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApplicantEducation(int id)
        {
            var ApplicantEducationDto = await applicantService.GetApplicantEducationById(id);
            if (ApplicantEducationDto == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return new JsonResult(new ResultDto<ApplicantEducationDto>(ApplicantEducationDto));
        }
        [HttpGet("DeleteApplicantEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteApplicantEducation(int id)
        {
            if (!await applicantService.DeleteApplicantEducation(id))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));


            return GetApplicanEducationListByEmail();
        }

        [HttpGet("GetApplicanEducationListByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicanEducationListByEmail()
        {
            var email = GetApplicantEmail();
            List<ApplicantEducationDto> model = applicantService.GetAllApplicantEducation(email);
            return new JsonResult(new ResultDto<List<ApplicantEducationDto>>(model));
        }
        #endregion
        #region Applicant Knowledge
        [HttpPost("CreateOrUpdateApplicantKnowledge")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateOrUpdateApplicantKnowledge(ApplicantKnowledgeDto knowledgeDto)
        {
            knowledgeDto.ApplicantEmail = GetApplicantEmail();
            if (!await applicantService.CreateOrUpdateKhnowledge(knowledgeDto))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicanKnowledgeListByEmail();
        }
        [HttpGet("GetApplicantKnowledge")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApplicantKnowledge(int id)
        {
            var ApplicantKnowledgeDto = await applicantService.GetApplicantKnowledgeById(id);
            if (ApplicantKnowledgeDto == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return new JsonResult(new ResultDto<ApplicantKnowledgeDto>(ApplicantKnowledgeDto));
        }
        [HttpGet("DeleteApplicantKnowledge")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteApplicantKnowledge(int id)
        {
            if (!await applicantService.DeleteApplicantKnowledge(id))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));


            return GetApplicanKnowledgeListByEmail();
        }
        [HttpGet("GetKnowledgeList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetKnowledgeList()
        {
            var knowledgeList = applicantService.GetKnowledgeList();
            return new JsonResult(new ResultDto<List<KnowledgeDto>>(knowledgeList));
        }

        [HttpGet("GetApplicanKnowledgeListByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicanKnowledgeListByEmail()
        {
            var email = GetApplicantEmail();
            List<ApplicantKnowledgeDto> model = applicantService.GetAllApplicantKnowledge(email);
            return new JsonResult(new ResultDto<List<ApplicantKnowledgeDto>>(model));
        }
        #endregion
        #region Applicant Language
        [HttpPost("CreateOrUpdateApplicantLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateOrUpdateApplicantLanguage(ApplicantLanguageDto languageDto)
        {
            languageDto.ApplicantEmail = GetApplicantEmail();
            if (!await applicantService.CreateOrUpdateLanguage(languageDto))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicanLanguageListByEmail();
        }
        [HttpGet("GetApplicantLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApplicantLanguage(int id)
        {
            var applicantLanguageDto = await applicantService.GetApplicantLanguageById(id);
            if (applicantLanguageDto == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return new JsonResult(new ResultDto<ApplicantLanguageDto>(applicantLanguageDto));
        }
        [HttpGet("DeleteApplicantLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteApplicantLanguage(int id)
        {
            if (!await applicantService.DeleteApplicantLanguage(id))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            return GetApplicanLanguageListByEmail();
        }

        [HttpGet("GetApplicanLanguageListByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicanLanguageListByEmail()
        {
            var email = GetApplicantEmail();
            List<ApplicantLanguageDto> model = applicantService.GetAllApplicantLanguage(email);
            return new JsonResult(new ResultDto<List<ApplicantLanguageDto>>(model));
        }
        [HttpGet("GetLanguageList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetLanguageList()
        {
            var LanguageList = applicantService.GetLanguageList();
            return new JsonResult(new ResultDto<List<LanguageDto>>(LanguageList));
        }
        #endregion
        #region PersonalInformation
        [HttpPost("CreateOrUpdateApplicantPersonalInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateOrUpdateApplicantPersonalInformation(ApplicantProfileDto applicantProfileDto)
        {
            applicantProfileDto.Email = GetApplicantEmail();
            if (!applicantService.SavePersonalInformation(applicantProfileDto).Result)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetPersonalInformation();
        }
        [HttpGet("GetPersonalInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetPersonalInformation()
        {
            var personalInformation = applicantService.GetPersonalInformation(GetApplicantEmail());
            if (personalInformation == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            return new JsonResult(new ResultDto<ApplicantDto>(personalInformation));
        }
        #endregion
        #region Applicant Contact Detail
        [HttpPost("CreateOrUpdateApplicantContactDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateOrUpdateApplicantContactDetail(ApplicantContactDetailsDto applicantContactDetailsDto)
        {
            if (!applicantService.SaveApplicantContactDetail(applicantContactDetailsDto, GetApplicantEmail()).Result)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return await GetApplicantContactDetail();
        }
        [HttpGet("GetApplicantContactDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApplicantContactDetail()
        {
            var allCountries = await Domain.Global.Global.GetCountryList(configurtion["GlobalSettings:ApiUrl"]);
            var allCities = await Domain.Global.Global.GetCityList(configurtion["GlobalSettings:ApiUrl"]);

            var applicantContactDetailsDto = applicantService.GetApplicantContactDetail(GetApplicantEmail());
            if (applicantContactDetailsDto == null) return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            applicantContactDetailsDto.CountryName = allCountries.SingleOrDefault(x => x.Code.Equals(applicantContactDetailsDto.CountryCode)).Name;
            if (applicantContactDetailsDto.CityId != null)
            {
                applicantContactDetailsDto.CityMainName = allCities.SingleOrDefault(x => x.Id == applicantContactDetailsDto.CityId).Name;
            }

            return new JsonResult(new ResultDto<ApplicantDto>(applicantContactDetailsDto));
        }
        #endregion
        #region Applicant Additional Section
        [HttpPost("CreateOrUpdateApplicantAdditionalSection")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateOrUpdateApplicantAdditionalSection(ApplicantAddtionalSectionDto addtionalSectionDto)
        {
            addtionalSectionDto.Email = GetApplicantEmail();
            if (!applicantService.SaveApplicantAdditionalSection(addtionalSectionDto).Result)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            return GetApplicantAdditionalSection();
        }
        [HttpGet("GetApplicantAdditionalSection")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicantAdditionalSection()
        {
            var applicantAdditionalSectionDto = applicantService.GetApplicantAdditionalSection(GetApplicantEmail());
            if (applicantAdditionalSectionDto == null)
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            return new JsonResult(new ResultDto<ApplicantDto>(applicantAdditionalSectionDto));
        }
        #endregion

        #region Offer
        [HttpGet("SearchOffers")]
        public IActionResult SearchOffers([FromQuery] List<string> jobCategories, string location,
            string language, string workPlace, string jobOffer, string employer, string position,
            string company, int currentPage = 1)
        {
            var model = applicantService.SearchOffersAjax(location, currentPage, language, workPlace, jobOffer, employer, position, company, jobCategories);

            return new JsonResult(new ResultDto<PaginationDto<OfferDto>>(model));
        }

        [HttpGet("SearchAllOffers")]
        public IActionResult SearchAllOffers()
        {
            var model = applicantService.SearchOffersAjax();

            return new JsonResult(new ResultDto<PaginationDto<OfferDto>>(model));
        }
        #endregion

        [HttpGet("GetPartialAsString")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ResultDto<string>> GetPartialAsString(string page)
        {
            var currentApplicant = await GetCurrentApplicant();
            var result = await renderer.RenderPartialToStringAsync(page, currentApplicant.Data);
            return new ResultDto<string>(result);
        }
        [HttpGet("SaveApplicantOfferFavourite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveApplicantOfferFavourite(int OfferId)
        {
            if (OfferId == 0) return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            var applicantOffersFavouriteDto = new ApplicantOffersFavouriteDto()
            {
                ApplicantEmail = GetApplicantEmail(),
                OfferId = OfferId
            };
            var result = await applicantService.SaveApplicantOfferFavourite(applicantOffersFavouriteDto);
            if (result == ApplicantOffersFavouriteResult.Success)
            {
                return new JsonResult(new ResultDto<ApplicantOffersFavouriteResult>(ApplicantOffersFavouriteResult.Success));
            }
            if (result == ApplicantOffersFavouriteResult.Badrequest)
            {
                return new JsonResult(new ResultDto<ApplicantOffersFavouriteResult>(ApplicantOffersFavouriteResult.Badrequest));
            }

            return new JsonResult(new ResultDto<ApplicantOffersFavouriteResult>(ApplicantOffersFavouriteResult.AddedBefore));
        }

        [HttpGet("GetAllFavourteApplicantOffers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllFavourteApplicantOffers()
        {
            var model = applicantService.GetAllFavourteApplicantOffers(GetApplicantEmail());
            return new JsonResult(new ResultDto<List<OfferDto>>(model));
        }
        [HttpGet("DeleteFavouriteOffer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFavouriteOffer(int id)
        {
            var result = await applicantService.DeleteFavouriteOffer(id, GetApplicantEmail());
            return new JsonResult(new ResultDto<bool>(result));
        }

        [HttpGet("GetCountFavouritApplicant")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCountFavouritApplicant()
        {
            var count= applicantService.GetCountFavouritApplicant(GetApplicantEmail());
            return new JsonResult(new ResultDto<int>(count));
        }
        [HttpGet("GetApplicantFavouriteOfferList")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetApplicantFavouriteOfferList()
        {
            var model = applicantService.applicantOffersFavouriteDtos(GetApplicantEmail());
            return new JsonResult(new ResultDto<List<ApplicantOffersFavouriteDto>>(model));
        }


    }
}