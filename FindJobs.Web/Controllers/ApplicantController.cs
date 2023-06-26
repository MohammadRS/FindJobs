using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Enums;
using DotNek.WebComponents.Areas.Dropdown.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Web.Helper;
using FindJobs.Web.Models;
using FindJobs.Web.Models.ApplicantViewModels;
using iText.Html2pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Res;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Path = System.IO.Path;


namespace FindJobs.Web.Controllers
{

    public class ApplicantController : BaseController
    {
        public ApplicantController(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<IActionResult> PreviewProfile(int id)

        {
            var resultOfRequest = await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetApplicantById?id=" + id, null);
            if (resultOfRequest.MessageCode == (int)MessageCodes.Success)
            {
                return View(resultOfRequest.Data);
            }
            else
            {
                return RedirectToAction("Page404", "Errors");
            }

        }

        [HttpGet()]
        public async Task<IActionResult> AjaxSearch(string key = "", string location = "", int currentPage = 1)
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
            var resultOfRequest = (await API.
                GetData<ResultDto<PaginationDto<ApplicantDto>>>(configuration["GlobalSettings:ApiUrl"],
                $"Applicant/GetApplicants?currentPage={currentPage}&key={key}&location={location}", Request.Cookies["AuthToken"])).Data;

            ViewData["CurrentPage"] = currentPage;
            return PartialView("_AjaxSearch", resultOfRequest);
        }

        [HttpGet()]
        public async Task<IActionResult> EmailPreferences()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
            var model = await InitializeViewModel();
            return View(model);
        }

        [HttpGet()]
        public async Task<IActionResult> Privacy()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
            var resultOfRequest = await API.GetData<ResultDto<ApplicantPrivacyDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetCurrentApplicant/ ", Request.Cookies["AuthToken"]);
            return View(resultOfRequest.Data);
        }
        [HttpPost()]
        public async Task<IActionResult> UpdatePrivacy(ApplicantPrivacyDto privacyDto)
        {
            await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                "Applicant/UpdatePrivacy",
                privacyDto, Request.Cookies["AuthToken"]);
            return RedirectToAction("Privacy");
        }
        [HttpPost()]
        public async Task<IActionResult> UpdateEmailPreferences(EmailPreferenceViewModel viewModel)
        {
            await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                "Applicant/UpdateEmailPreferences",
                viewModel, Request.Cookies["AuthToken"]);
            var model = await InitializeViewModel();
            return RedirectToAction("EmailPreferences",model);
        }


        [Route("Unsubscribe/{EncryptedEmail}")]
        [HttpGet]
        public async Task<IActionResult> Unsubscribe(string EncryptedEmail)
        {

            var result = await API.GetData<ResultDto>(configuration["GlobalSettings:ApiUrl"], "Applicant/UnsubscribeEmailPreferences/" + HttpUtility.UrlEncode(EncryptedEmail));


            ViewData["Message"] = Messages.ResourceManager.GetString(Enum.GetName(typeof(MessageCodes), result.MessageCode));
            return View();
        }
        public async Task<IActionResult> CalculationProgressbar()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
            var resultOfRequest = (await API.GetData<ResultDto<CalculateProgressbarDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/CalculationProgressbar", Request.Cookies["AuthToken"])).Data;
            return PartialView("_ProgressBar", resultOfRequest);
        }


        public async Task<IActionResult> Profile(string token=null)
        {
           
            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMonths(3) });
                if (!await CheckUserAccess.IsUserHasAccess(token, ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
                var resultOfRequest = (await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetCurrentApplicant", token)).Data;
                return View(resultOfRequest);
            }
            else
            {
                if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");
                var resultOfRequest = (await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetCurrentApplicant", Request.Cookies["AuthToken"])).Data;
                return View(resultOfRequest);
            }
        }

        [HttpGet("Offer/{id}")]
        public async Task<IActionResult> Offer(int id)
        {
            var result = await API.GetData<GenericRespnseApi<OfferDto>>(configuration["GlobalSettings:ApiUrl"], "Company/GetOffer?id=" + id, Request.Cookies["AuthToken"]);
            if (result is not null)
            {
                return View(result.Data);
            }
            return BadRequest();
        }

        private async Task<EmailPreferenceViewModel> InitializeViewModel()
        {
            var model = new EmailPreferenceViewModel();
            var email = HttpContext.Session.GetString("Email");
            if (email is null)
            {
                var result = await API.GetData<ApplicantDto>(configuration["GlobalSettings:ApiUrl"], "Applicant/GetCurrentApplicant",
                    Request.Cookies["AuthToken"]);
                if (result is not null)
                {
                    model.ApplicantEmail = result.Email;
                }
            }
            else
                model.ApplicantEmail = email;


            var checkboxes = await InitializeCheckboxes();
            model.JobNotification = checkboxes.JobNotification;
            model.NewsLetter = checkboxes.NewsLetter;

            return model;
        }
        private async Task<EmailPreferenceViewModel> InitializeCheckboxes()
        {
            var model = new EmailPreferenceViewModel();

            var result = await API.GetData<ResultDto<List<ApplicantPreferenceDto>>>(configuration["GlobalSettings:ApiUrl"],
                "Applicant/GetEmailPreferences", Request.Cookies["AuthToken"]);
            if (result is not null)
            {
                foreach (var emailPreferenceViewModel in result.Data)
                {
                    switch (emailPreferenceViewModel.Category)
                    {
                        case "NewsLetter":
                            if (emailPreferenceViewModel.IsSubscribed) model.NewsLetter = "checked";
                            break;
                        case "JobNotification":
                            if (emailPreferenceViewModel.IsSubscribed) model.JobNotification = "checked";
                            break;
                    }
                }
            }
            return model;
        }
        public async Task<IActionResult> CV()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");

            ViewData["LanguageDropDownModel"] = new DropdownConfiguration()
            {
                SelectStatus = "custom-select-status",
                ComponentId = "ddlLang",
                Title = Res.ApplicantProfile.CvLanguage,
                Culture = ViewData["Culture"].ToString(),
                SelectedItemColor = ViewData["ColorAqua"].ToString(),
                Items = ((List<CultureDto>)ViewData["CultureList"]).Select(x => x.LanguageNativeName).Distinct().ToList()
            };
            var applicantModel = (await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetCurrentApplicant", Request.Cookies["AuthToken"])).Data;
            return View(applicantModel);
        }
        public async Task<string> ConvertImageFileToBase64(IFormFile applicantImage)
        {
            using (var memoryStream = new MemoryStream())
            {
                await applicantImage.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        public async Task<string> UploadImage([FromForm] IFormCollection userImage)
        {
            HttpContext.Session.Remove("ApplicantImage");
            var imageBase64 = await ConvertImageFileToBase64(userImage.Files[0]);
            var result = await API.PostData<ResultDto>(configuration["GlobalSettings:ApiUrl"],
           "Applicant/UpdateApplicantImage", imageBase64, Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                return imageBase64;
            }
            return "";
        }

        public async Task<IActionResult> getCountry([FromQuery] string value)
        {
            var allCountries = await Domain.Global.Global.GetCountryList(configuration["GlobalSettings:ApiUrl"]);
            var countries = allCountries.Where(x => x.Name.Contains(value));
            return new JsonResult(new { countries = allCountries });
        }

        public async Task<IActionResult> getCities([FromQuery] string value)
        {
            var allCountries = await Domain.Global.Global.GetCountryList(configuration["GlobalSettings:ApiUrl"]);
            var allCities = await Domain.Global.Global.GetCityList(configuration["GlobalSettings:ApiUrl"]);
            var country = allCountries.FirstOrDefault(x => x.Name.Equals(value));
            var cities = allCities.Where(x => x.CountryCode.Equals(country.Code));
            return new JsonResult(new { cities = cities });
        }

        public async Task<IActionResult> GetCityList([FromQuery] string value)
        {
            var allCities = await Domain.Global.Global.GetCityList(configuration["GlobalSettings:ApiUrl"]);
            if (value != null)
            {
                var cities = allCities.Where(x => x.Name.ToLower().StartsWith(value));
                return new JsonResult(new { cities = cities });
            }
            else
            {
                return new JsonResult(new { cities = new List<CityDto>() });
            }

        }

        public async Task<IActionResult> GetStates([FromQuery] string value)
        {
            var allCountries = await Domain.Global.Global.GetCountryList(configuration["GlobalSettings:ApiUrl"]);
            var allCities = await Domain.Global.Global.GetCityList(configuration["GlobalSettings:ApiUrl"]);
            var country = allCountries.FirstOrDefault(x => x.Name.Equals(value));
            var states = allCities.Where(x => x.CountryCode.Equals(country.Code)).Select(x => x.StateName).Distinct();
            return new JsonResult(new { states = states });
        }

        public async Task<IActionResult> getCitiesByState([FromQuery] string value)
        {
            var allCities = await Domain.Global.Global.GetCityList(configuration["GlobalSettings:ApiUrl"]);
            var cities = allCities.Where(x => x.StateName.Equals(value)).ToList();
            return new JsonResult(new { cities = cities });
        }


        #region ApplicantDocuments


        public async Task<IActionResult> UploadDocument([FromForm] ApplicantDocumentViewModel documentViewModel)
        {
            var applicantDocumentDto = new ApplicantDocumentsDto
            {
                DocumentFile = await ConvertImageFileToBase64(documentViewModel.DocumentFile),
                ExtensionFile = Path.GetExtension(documentViewModel.DocumentFile.FileName),
                Type = documentViewModel.Type
            };

            var result = await API.PostData<ResultDto<List<ApplicantDocumentsDto>>>(configuration["GlobalSettings:ApiUrl"],
           "Applicant/InsertApplicantDocument"
           , applicantDocumentDto, Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantDocumentList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }
        private string ConvertFileToByteArray(IFormFile file)
        {

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                bytes = stream.ToArray();
            }
            return bytes.ToString();

        }

        [HttpGet("GetApplicantDocuments")]
        public async Task<IActionResult> GetApplicantDocuments()
        {
            var result = await API.GetData<ResultDto<List<ApplicantDocumentsDto>>>(configuration["GlobalSettings:ApiUrl"],
          "Applicant/GetApplicantDocumentsByEmail"
          , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantDocumentList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }
        public async Task<IActionResult> DeleteApplicantDocument(int id)
        {
            var result = await API.GetData<ResultDto<List<ApplicantDocumentsDto>>>(configuration["GlobalSettings:ApiUrl"],
        "Applicant/DeleteApplicantDocument?id=" + id
        , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantDocumentList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }

        #endregion

        #region WorkExperience
        [HttpGet]
        public async Task<IActionResult> GetWorkExperience(int id = 0)
        {
            if (id == 0)
            {
                var model = new ApplicantWorkExperienceDto();
                return PartialView("_InsertOrUpdateWorkExperience", model);
            }
            else
            {
                var result = await API.GetData<ResultDto<ApplicantWorkExperienceDto>>(configuration["GlobalSettings:ApiUrl"],
                    "Applicant/GetWorkExperienceById?id=" + id
                    , Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_InsertOrUpdateWorkExperience", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
        }
        public async Task<IActionResult> InsertOrUpdateWorkExperience([FromForm] ApplicantWorkExperienceDto workExperienceDto)
        {
            if(workExperienceDto.EndWork < workExperienceDto.StartWork)
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }

            if (workExperienceDto.Id != 0)
            {
                var result = await API.PostData<ResultDto<List<ApplicantWorkExperienceDto>>>(configuration["GlobalSettings:ApiUrl"],
             "Applicant/InsertOrUpdateWorkExperience"
             , workExperienceDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantWorkExperienceList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
            else
            {
                var result = await API.PostData<ResultDto<List<ApplicantWorkExperienceDto>>>(configuration["GlobalSettings:ApiUrl"],
              "Applicant/InsertOrUpdateWorkExperience"
              , workExperienceDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantWorkExperienceList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
        }
        public async Task<IActionResult> RemoveApplicantWorkExperience(int id)
        {
            var result = await API.GetData<ResultDto<List<ApplicantWorkExperienceDto>>>(configuration["GlobalSettings:ApiUrl"],
       "Applicant/DeleteWorkExperience?id=" + id
       , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantWorkExperienceList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }

        #endregion

        #region Applicant Education
        [HttpGet]
        public async Task<IActionResult> GetApplicantEducation(int id = 0)
        {
            if (id == 0)
            {
                var model = new ApplicantEducationDto();
                return PartialView("_CreateOrUpdateEducation", model);
            }
            else
            {
                var result = await API.GetData<ResultDto<ApplicantEducationDto>>(configuration["GlobalSettings:ApiUrl"],
                    "Applicant/GetApplicantEducation?id=" + id
                    , Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_CreateOrUpdateEducation", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
        }
        public async Task<IActionResult> CreateOrUpdateApplicantEducation([FromForm] ApplicantEducationDto applicantEducationDto)
        {
            if (applicantEducationDto.EndEducation < applicantEducationDto.StartEducation)
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
            if (applicantEducationDto.Id != 0)
            {
                var result = await API.PostData<ResultDto<List<ApplicantEducationDto>>>(configuration["GlobalSettings:ApiUrl"],
             "Applicant/CreateOrUpdateApplicantEducation"
             , applicantEducationDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantEducations", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }

            else
            {
                var result = await API.PostData<ResultDto<List<ApplicantEducationDto>>>(configuration["GlobalSettings:ApiUrl"],
              "Applicant/CreateOrUpdateApplicantEducation"
              , applicantEducationDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantEducations", model);

                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
        }
        public async Task<IActionResult> RemoveApplicantEducation(int id)
        {
            var result = await API.GetData<ResultDto<List<ApplicantEducationDto>>>(configuration["GlobalSettings:ApiUrl"],
       "Applicant/DeleteApplicantEducation?id=" + id
       , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantEducations", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }

        #endregion
        #region Applicant Knowledge
        [HttpGet]
        public async Task<IActionResult> GetApplicantKnowledge(int id = 0)
        {
            var resultKnolegeList = await API.GetData<ResultDto<List<KnowledgeDto>>>(configuration["GlobalSettings:ApiUrl"],
                  "Applicant/GetKnowledgeList", Request.Cookies["AuthToken"]);
            if (id == 0)
            {

                var model = new ApplicantKnowledgeDto();
                model.KnowledgeDtos = resultKnolegeList.Data;
                return PartialView("_CreateOrUpdateKnowledge", model);
            }
            else
            {
                var result = await API.GetData<ResultDto<ApplicantKnowledgeDto>>(configuration["GlobalSettings:ApiUrl"],
                    "Applicant/GetApplicantKnowledge?id=" + id
                    , Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = new ApplicantKnowledgeDto();
                    model = result.Data;
                    model.KnowledgeDtos = resultKnolegeList.Data.OrderBy(x => x.Name).ToList();
                    return PartialView("_CreateOrUpdateKnowledge", model);
                }
            }

            return PartialView("_ApplicantEducations");
        }
        public async Task<IActionResult> CreateOrUpdateApplicantKnowledge([FromForm] ApplicantKnowledgeDto knowledgeDto)
        {
            if (knowledgeDto.Id != 0)
            {
                var result = await API.PostData<ResultDto<List<ApplicantKnowledgeDto>>>(configuration["GlobalSettings:ApiUrl"],
             "Applicant/CreateOrUpdateApplicantKnowledge"
             , knowledgeDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {

                    var model = result.Data;
                    return PartialView("_ApplicantKnowledgeList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }

            }
            else
            {
                var result = await API.PostData<ResultDto<List<ApplicantKnowledgeDto>>>(configuration["GlobalSettings:ApiUrl"],
              "Applicant/CreateOrUpdateApplicantKnowledge"
              , knowledgeDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantKnowledgeList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }

            }
        }
        public async Task<IActionResult> RemoveApplicantKnowledge(int id)
        {
            var result = await API.GetData<ResultDto<List<ApplicantKnowledgeDto>>>(configuration["GlobalSettings:ApiUrl"],
       "Applicant/DeleteApplicantKnowledge?id=" + id
       , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantKnowledgeList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }

        }


        #endregion
        #region Applicant Language
        [HttpGet]
        public async Task<IActionResult> GetApplicantLanguage(int id = 0)
        {
            var resultLanguageList = await API.GetData<ResultDto<List<LanguageDto>>>(configuration["GlobalSettings:ApiUrl"],
                 "Applicant/GetLanguageList", Request.Cookies["AuthToken"]);
            if (id == 0)
            {

                var model = new ApplicantLanguageDto();
                model.languageDtos = resultLanguageList.Data.ToList();
                return PartialView("_CreateOrUpdateLanguage", model);
            }
            else
            {
                var result = await API.GetData<ResultDto<ApplicantLanguageDto>>(configuration["GlobalSettings:ApiUrl"],
                    "Applicant/GetApplicantLanguage?id=" + id
                    , Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    model.languageDtos = resultLanguageList.Data.OrderByDescending(x => x.Importance).ToList();
                    return PartialView("_CreateOrUpdateLanguage", model);
                }
            }

            return PartialView("_ApplicantLanguageList");
        }
        public async Task<IActionResult> CreateOrUpdateApplicantLanguage([FromForm] ApplicantLanguageDto languageDto)
        {
            if (languageDto.Id != 0)
            {
                var result = await API.PostData<ResultDto<List<ApplicantLanguageDto>>>(configuration["GlobalSettings:ApiUrl"],
             "Applicant/CreateOrUpdateApplicantLanguage"
             , languageDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantLanguageList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
            else
            {
                var result = await API.PostData<ResultDto<List<ApplicantLanguageDto>>>(configuration["GlobalSettings:ApiUrl"],
              "Applicant/CreateOrUpdateApplicantLanguage"
              , languageDto, Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == (int)MessageCodes.Success)
                {
                    var model = result.Data;
                    return PartialView("_ApplicantLanguageList", model);
                }
                else
                {
                    return new JsonResult(new
                    {
                        html = "",
                        success = "Error"
                    });
                }
            }
        }
        public async Task<IActionResult> RemoveApplicantLanguage(int id)
        {
            var result = await API.GetData<ResultDto<List<ApplicantLanguageDto>>>(configuration["GlobalSettings:ApiUrl"],
       "Applicant/DeleteApplicantLanguage?id=" + id
       , Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_ApplicantLanguageList", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }
        #endregion

        #region Personal Information
        public async Task<IActionResult> CreateOrUpdateApplicantPersonalInformation([FromForm] ApplicantProfileDto profileDto)
        {
            if (profileDto.ReadyToWorkStatus.Equals(ReadyToWork.ASAP))
            {
                profileDto.AvailableDate = null;
            }
            var result = await API.PostData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"],
         "Applicant/CreateOrUpdateApplicantPersonalInformation"
         , profileDto, Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_PersonalInformation", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }
        #endregion
        #region Applicant Contact Detail
        public async Task<IActionResult> CreateOrUpdateApplicantContactDetail([FromForm] ApplicantContactDetailsDto contactDetailsDto)
        {
            var allCountries = await Domain.Global.Global.GetCountryList(configuration["GlobalSettings:ApiUrl"]);
            var allCities = await Domain.Global.Global.GetCityList(configuration["GlobalSettings:ApiUrl"]);
            if (contactDetailsDto.CityMainName != null)
            {
                contactDetailsDto.CityName = null;
            }

            if (contactDetailsDto.CountryName != null)
                contactDetailsDto.CountryCode = allCountries.SingleOrDefault(x => x.Name.Equals(contactDetailsDto.CountryName)).Code;
            if (contactDetailsDto.CityMainName != null && contactDetailsDto.StateName != null)
            {
                contactDetailsDto.CityId = allCities.SingleOrDefault(x => x.Name.Equals(contactDetailsDto.CityMainName) && x.StateName.Equals(contactDetailsDto.StateName)).Id;
            }
            if (contactDetailsDto.CityMainName != null && contactDetailsDto.StateName == null)
            {
                contactDetailsDto.CityId = allCities.SingleOrDefault(x => x.Name.Equals(contactDetailsDto.CityMainName)).Id;
            }


            var result = await API.PostData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"],
         "Applicant/CreateOrUpdateApplicantContactDetail"
         , contactDetailsDto, Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;


                return PartialView("_ContactDetail", model);

            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }
        }
        #endregion
        #region Applicant Additional Section
        public async Task<IActionResult> CreateOrUpdateApplicantAdditionalSection([FromForm] ApplicantAddtionalSectionDto addtionalSectionDto)
        {
            if (addtionalSectionDto.IsHourlyRate)
            {
                addtionalSectionDto.HourlyAverage = 0;
            }
            else
            {
                addtionalSectionDto.HourlyFrom = 0;
                addtionalSectionDto.HourlyUntil = 0;

            }
            if (addtionalSectionDto.IsDrivingLicense == false)
            {
                addtionalSectionDto.LicenseType = null;
            }
            var result = await API.PostData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"],
         "Applicant/CreateOrUpdateApplicantAdditionalSection"
         , addtionalSectionDto, Request.Cookies["AuthToken"]);
            if (result != null && result.MessageCode == (int)MessageCodes.Success)
            {
                var model = result.Data;
                return PartialView("_AdditionalSection", model);
            }
            else
            {
                return new JsonResult(new
                {
                    html = "",
                    success = "Error"
                });
            }

        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> AppliedJobs()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Applicant, configuration)) return RedirectToAction("Index", "Home");

            return View();
        }



        #region PDfConvertor
        [HttpGet]
        public async Task<IActionResult> PdfConvertorAsync(string page)
        {
            Byte[] res = null;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            page = $"/Views/Cvs/_{page}.cshtml";

            var applicantModel = (await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetCurrentApplicant", Request.Cookies["AuthToken"])).Data;

            var html = await API.GetData<ResultDto<string>>(configuration["GlobalSettings:ApiUrl"],
               $"Applicant/GetPartialAsString?page={page}", Request.Cookies["AuthToken"]);

            using (MemoryStream ms = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(html.Data, ms);
                res = ms.ToArray();
            }




            return File(res, "application/pdf");
        }
        #endregion
    }
}
