using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Enums;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Domain.ViewModels;
using FindJobs.Web.Helper;
using FindJobs.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NUglify.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private new readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IStringLocalizer<HomeController> localizer) : base(configuration)
        {
            _logger = logger;
            this.configuration = configuration;

        }

        public async Task<IActionResult> ListOffer(string jobCategories, string location)
        {
            var apiUrl = configuration["GlobalSettings:ApiUrl"];

            var result = await API.GetData<ResultDto<PaginationDto<OfferDto>>>(apiUrl,
                  $"Applicant/SearchOffers?location={location}&jobCategories={jobCategories}");

            return View(result.Data);

        }

        public async Task<IActionResult> ListOfferAdvance(string jobCategories, string location,
          string language, string workPlace, string jobOffer, string employer, string position, string company, string currentPage)
        {
            var apiUrl = configuration["GlobalSettings:ApiUrl"];

            var parameters = "";
            if (jobCategories != null)
            {
                var jobcategoriesArray = jobCategories.Split(',');
                ;
                foreach (var jobCategory in jobcategoriesArray)
                {
                    parameters += "&jobCategories=" + jobCategory;
                }
            }

            var result = await API.GetData<ResultDto<PaginationDto<OfferDto>>>(apiUrl,
                 $"Applicant/SearchOffers?jobCategories={parameters}&location={location}&language={language}" +
                 $"&workPlace={workPlace}&jobOffer={jobOffer}&employer={employer}&position={position}&company={company}&currentPage={currentPage}");
            return PartialView("_ListOffer", result.Data);
        }

        public async Task<IActionResult> ListOfferAjax(string currentPage)
        {

            var apiUrl = configuration["GlobalSettings:ApiUrl"];

            var result = await API.GetData<ResultDto<PaginationDto<OfferDto>>>(apiUrl,
                  $"Applicant/SearchOffers?currentPage={currentPage}");


            if (result.MessageCode == 0)
            {
                return PartialView("_ListOffer", result.Data);
            }
            return PartialView("_ListOffer");

        }

        public async Task<IActionResult> Index()
        {

            var result = await API.GetData<ResultDto<List<CompanyDto>>>(configuration["GlobalSettings:ApiUrl"], "Company/GetTopEmployers");
            if (result == null)
                return View();
            if (result.MessageCode == (int)MessageCodes.Success)
            {
                ViewData["TopEmployers"] = result.Data;
            }
            else
            {
                ViewData["TopEmployers"] = null;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public async Task<IActionResult> Preview( int id)
        {
            var offer = await API.GetData<ResultDto<OfferDto>>(configuration["GlobalSettings:ApiUrl"],
               $"Company/GetOffer?id={id}", Request.Cookies["AuthToken"]);

            var documents = await API.GetData<ResultDto<List<ApplicantDocumentsDto>>>(configuration["GlobalSettings:ApiUrl"],
               $"Applicant/GetApplicantDocumentsByEmail", Request.Cookies["AuthToken"]);

            var offerviewModel = new OfferViewModel
            {
                offerDto = offer.Data,
                applicantDocumentsDtos = documents.Data
            };

            return View(offerviewModel);
        }
        public static List<JobCategoryCountDto> GetJobCategoryList(List<OfferDto> offerDtos)
        {
            List<JobCategoryCountDto> JobCategoryCountList = new List<JobCategoryCountDto>();
            var jobCategories = offerDtos.SelectMany(x => x.OfferJobCategoryDtos).Select(s => s.JobCategoryDto.Jobcategory).Distinct();
            var query1 = offerDtos.SelectMany(x => x.OfferJobCategoryDtos).Select(s => new
            {
                job = s.JobCategoryDto.Jobcategory,
                offerId = s.OfferId,
                jobId = s.JobCategoryId
            });
            int sum = 0;
            int categoryId = 0;
            foreach (var item in jobCategories)
            {
                foreach (var item2 in query1)
                {
                    if (item == item2.job.ToString())
                    {
                        sum++;
                        categoryId = item2.jobId;
                    }
                }
                var jobCategoryCount = new JobCategoryCountDto();
                jobCategoryCount.Id = categoryId;
                jobCategoryCount.JobCategory = item.ToString();
                jobCategoryCount.Count = sum;
                JobCategoryCountList.Add(jobCategoryCount);
                sum = 0;
            }
            return JobCategoryCountList;
        }
        public static List<EmploymentCountDto> GetEmployTypeList(List<OfferDto> offerDtos)
        {
            List<EmploymentCountDto> EmploymentDto = new List<EmploymentCountDto>();
            var emp = offerDtos.Select(s => new
            {
                s.Id,
                s.IsFullTime,
                s.IsPartTime,
                s.Agreement,
                s.Freelancer,
                s.IsInternShip

            }).ToList();

            int FullTimeCount = 0;
            int PartTimeCount = 0;
            int AgreementCount = 0;
            int FreeLancerCount = 0;
            int InternShipCount = 0;

            foreach (var item in emp)
            {
                if (item.IsFullTime)
                    FullTimeCount++;
                if (item.IsPartTime)
                    PartTimeCount++;
                if (item.Agreement)
                    AgreementCount++;
                if (item.Freelancer)
                    FreeLancerCount++;
                if (item.IsInternShip)
                    InternShipCount++;
            }
            if (emp.Any(x => x.IsFullTime))
            {
                EmploymentDto.Add(new EmploymentCountDto { Id = emp.FirstOrDefault(x => x.IsFullTime).Id, Name = Res.CompanyOffer.FullTime, Count = FullTimeCount });
            }
            if (emp.Any(x => x.IsPartTime))
            {
                EmploymentDto.Add(new EmploymentCountDto { Id = emp.FirstOrDefault(x => x.IsPartTime).Id, Name = Res.CompanyOffer.PartTime, Count = PartTimeCount });
            }
            if (emp.Any(x => x.Agreement))
            {
                EmploymentDto.Add(new EmploymentCountDto { Id = emp.FirstOrDefault(x => x.Agreement).Id, Name = Res.CompanyOffer.Agreement, Count = AgreementCount });
            }
            if (emp.Any(x => x.Freelancer))
            {
                EmploymentDto.Add(new EmploymentCountDto { Id = emp.FirstOrDefault(x => x.Freelancer).Id, Name = Res.CompanyOffer.Freelancer, Count = FreeLancerCount });
            }
            if (emp.Any(x => x.IsInternShip))
            {
                EmploymentDto.Add(new EmploymentCountDto { Id = emp.FirstOrDefault(x => x.IsInternShip).Id, Name = Res.CompanyOffer.InternShip, Count = InternShipCount });
            }


            return EmploymentDto;

        }
        public static List<LanguageCountDto> GetLanguageList(List<OfferDto> offerDtos)
        {
            List<LanguageCountDto> languageCountDto = new List<LanguageCountDto>();
            var language = offerDtos.SelectMany(x => x.OfferLanguageDtos).Select(s => s.LanguageDto.Name).Distinct();
            var query1 = offerDtos.SelectMany(x => x.OfferLanguageDtos).Select(s => new
            {
                lang = s.LanguageDto.Name,
                offerId = s.OfferId,
                langId = s.LanguageId
            });
            int sum = 0;
            int languageId = 0;
            foreach (var item in language)
            {
                foreach (var item2 in query1)
                {
                    if (item == item2.lang.ToString())
                    {
                        sum++;
                        languageId = item2.langId;
                    }
                }
                var languageCounter = new LanguageCountDto();
                languageCounter.Id = languageId;
                languageCounter.Name = item;
                languageCounter.Count = sum;
                languageCountDto.Add(languageCounter);
                sum = 0;
            }
            return languageCountDto;
        }
        public static List<PackageCountDto> GetPackageList(List<OfferDto> offerDtos)
        {
            List<PackageCountDto> offerPackageCountDtos = new List<PackageCountDto>();
            var packages = offerDtos.Select(s => new
            {
                s.Id,
                s.PackageName,
                count = 0

            }).ToList();


            foreach (var item in packages)
            {
                if (!offerPackageCountDtos.Any(x => x.Name.Equals(item.PackageName)))
                    offerPackageCountDtos.Add(new PackageCountDto { Id = item.Id, Name = item.PackageName, Count = 1 });
                else
                    offerPackageCountDtos.Find(x => x.Name.Equals(item.PackageName)).Count++;

            }
            return offerPackageCountDtos;

        }
        public static List<WorkingAreaCountDto> GetWorkingAreaList(List<OfferDto> offerDtos)
        {
            List<WorkingAreaCountDto> workingAreaCountDto = new List<WorkingAreaCountDto>();
            var area = offerDtos.Select(s => new
            {
                s.Id,
                s.IsWorkingFromHome,
                s.IsWorkAtTheRegularWorkPlace,
                s.IsPartialWorkFromHome

            }).ToList();

            int FromHomeCount = 0;
            int RegularPlaceCount = 0;
            int PartialCount = 0;

            foreach (var item in area)
            {
                if (item.IsWorkingFromHome)
                    FromHomeCount++;
                if (item.IsWorkAtTheRegularWorkPlace)
                    RegularPlaceCount++;
                if (item.IsPartialWorkFromHome)
                    PartialCount++;
            }
            if (area.Any(x => x.IsWorkingFromHome))
            {
                workingAreaCountDto.Add(new WorkingAreaCountDto { Id = area.FirstOrDefault(x => x.IsWorkingFromHome).Id, Name = Res.CompanyOffer.PartialWorkFromHome, Count = FromHomeCount });
            }
            if (area.Any(x => x.IsWorkAtTheRegularWorkPlace))
            {
                workingAreaCountDto.Add(new WorkingAreaCountDto { Id = area.FirstOrDefault(x => x.IsWorkAtTheRegularWorkPlace).Id, Name = Res.CompanyOffer.WorkAtTheRegularWorkPlace, Count = RegularPlaceCount });
            }
            if (area.Any(x => x.IsPartialWorkFromHome))
            {
                workingAreaCountDto.Add(new WorkingAreaCountDto { Id = area.FirstOrDefault(x => x.IsPartialWorkFromHome).Id, Name = Res.CompanyOffer.PartialWorkFromHome, Count = PartialCount });
            }



            return workingAreaCountDto;

        }
        public static List<CompanyCountDto> GetCompanyList(List<OfferDto> offerDtos)
        {
            List<CompanyCountDto> companiesCountList = new List<CompanyCountDto>();

            var companies = offerDtos.Select(s => new
            {
                s.CompanyDto.Name,
                s.CompanyDto.Id
            }).ToList();

            foreach (var item in companies)
            {
                if (!companiesCountList.Any(x => x.Name.Equals(item.Name)))
                    companiesCountList.Add(new CompanyCountDto { Id = item.Id, Name = item.Name, Count = 1 });
                else
                    companiesCountList.Find(x => x.Name.Equals(item.Name)).Count++;

            }

            return companiesCountList;



        }

        public static List<string> GetEducationList(OfferDto model)
        {
            List<string> educationList = new List<string>();
            if (model.HasPrimaryEducation)
            {
                educationList.Add(Res.CompanyOffer.Primary_education);
            }
            if (model.HasHighSchoolStudent)
            {
                educationList.Add(Res.CompanyOffer.Highschoollstudent);

            }
            if (model.HasSecondarySchoolWithGraduation)
            {
                educationList.Add(Res.CompanyOffer.Secondaryschoollwithgraduation);

            }
            if (model.HasSecondarySchoolWithOutGraduation)
            {
                educationList.Add(Res.CompanyOffer.Secondaryschoollwithoutgraduation);
            }
            if (model.HasPostSecondary)
            {
                educationList.Add(Res.CompanyOffer.PostSecondary);
            }
            if (model.HasUniversityStudent)
            {
                educationList.Add(Res.CompanyOffer.UniversityStudent);
            }
            if (model.HasHigherEducationLevel1)
            {
                educationList.Add(Res.CompanyOffer.HighereducationLevel1);
            }
            if (model.HasHigherEducationLevel2)
            {
                educationList.Add(Res.CompanyOffer.HighereducationLevel2);
            }
            if (model.HasHigherEducationLevel3)
            {
                educationList.Add(Res.CompanyOffer.HighereducationLevel3);
            }


            return educationList;
        }

        public static List<string> GetTypeOfContractList(OfferDto model)
        {
            List<string> contractType = new List<string>();
            if (model.IsFullTime)
            {
                contractType.Add(Res.CompanyOffer.FullTime);
            }
            if (model.IsPartTime)
            {
                contractType.Add(Res.CompanyOffer.PartTime);

            }
            if (model.IsPartialWorkFromHome)
            {
                contractType.Add(Res.CompanyOffer.PartialWorkFromHome);

            }
            if (model.Freelancer)
            {
                contractType.Add(Res.CompanyOffer.Freelancer);
            }
            if (model.Agreement)
            {
                contractType.Add(Res.CompanyOffer.Agreement);
            }
            if (model.IsInternShip)
            {
                contractType.Add(Res.CompanyOffer.InternShip);
            }
            return contractType;
        }

        public static List<string> GetLicenceType(OfferDto Model)
        {
            List<string> Licence = new List<string>();
            if (Model.HasDriverLicenceA)
            {
                Licence.Add(@Res.CompanyOffer.A);
            }
            if (Model.HasDriverLicenceB)
            {
                Licence.Add(@Res.CompanyOffer.B);
            }
            if (Model.HasDriverLicenceC)
            {
                Licence.Add(@Res.CompanyOffer.C);
            }
            if (Model.HasDriverLicenceD)
            {
                Licence.Add(@Res.CompanyOffer.D);
            }
            if (Model.HasDriverLicenceE)
            {
                Licence.Add(@Res.CompanyOffer.E);
            }


            return Licence;
        }
    }


}
