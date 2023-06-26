using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Dtos.Email;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FindJobs.Domain.ViewModels;
using System;

using DotNek.Common.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Internal;
using Xamarin.Forms;

namespace FindJobs.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMailSenderService senderService;
        private readonly IRazorPartialToStringService renderer;
        private readonly IGenericRepository<Company> companyRepository;
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IGenericRepositorySimple<JobCategory> jobCategoryRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IEncryption encryption;
        private readonly IGenericRepository<Language> languageRepository;
        private readonly IGenericRepository<Knowledge> knowledgeRepository;
        private readonly IGenericRepository<Currency> currencyRepository;
        private readonly IGenericRepository<Package> packageRepository;
        private readonly IGenericRepository<OfferLanguage> offerLanguageRepository;
        private readonly IGenericRepository<OfferKnowledge> offerKnowledgeRepository;
        private readonly IGenericRepository<OfferJobCategory> offerJobCategoryRepository;
        private readonly IGenericRepository<City> cityRepositor;


        // private readonly ControllerContext controllerContext;

        public CompanyService(IMailSenderService senderService,
            IRazorPartialToStringService renderer,
            IGenericRepository<Company> companyRepository,
           IGenericRepository<Offer> offerRepository,
           IGenericRepositorySimple<JobCategory> jobCategoryRepository,
           IMapper mapper,
           IConfiguration configuration,
           IEncryption encryption,
           IGenericRepository<Language> LanguageRepository,
           IGenericRepository<Knowledge> KnowledgeRepository,
           IGenericRepository<Currency> CurrencyRepository,
           IGenericRepository<Package> PackageRepository,
             IGenericRepository<OfferLanguage> OfferLanguageRepository,
              IGenericRepository<OfferKnowledge> OfferKnowledgeRepository,
              IGenericRepository<OfferJobCategory> offerJobCategoryRepository,
              IGenericRepository<City> cityRepositor

            )
        {

            this.senderService = senderService;
            this.renderer = renderer;
            this.companyRepository = companyRepository;
            this.offerRepository = offerRepository;
            this.jobCategoryRepository = jobCategoryRepository;
            this.mapper = mapper;
            this.configuration = configuration;
            this.encryption = encryption;
            languageRepository = LanguageRepository;
            knowledgeRepository = KnowledgeRepository;
            currencyRepository = CurrencyRepository;
            packageRepository = PackageRepository;
            offerLanguageRepository = OfferLanguageRepository;
            offerKnowledgeRepository = OfferKnowledgeRepository;
            this.offerJobCategoryRepository = offerJobCategoryRepository;
            this.cityRepositor = cityRepositor;
        }

        public async Task<EmailSendResult> SendOfferEmails(string email)
        {
            var mailModel = new MailModelDto();
            mailModel.Email = email;
            var offers = offerRepository.GetEntities().ToList();
            var offerDtoList = mapper.Map<List<OfferDto>>(offers);
            SendOfferEmail sendOfferEmail = new SendOfferEmail
            {
                Model = offerDtoList,
                HeaderTitle = Res.Email.HeaderTitle,
                HeaderSubTitle = Res.Email.HeaderSubTitle,
                InstagramLink = configuration["GlobalSettings:InstagramLink"],
                FacebookLink = configuration["GlobalSettings:FacebookLink"],
                TwitterLink = configuration["GlobalSettings:TwitterLink"],
            };
            var encryptedEmail = HttpUtility.UrlEncode(encryption.Encrypt(email, configuration["GlobalSettings:EncryptionSalt"]));

            sendOfferEmail.UnsubscribeLink = configuration["GlobalSettings:WebUrl"] + "UnSubscribe/" + encryptedEmail;
            List<string> links = new List<string>();
            foreach (var item in offerDtoList)
            {
                links.Add(item.Link = configuration["GlobalSettings:WebUrl"] + "Offer/" + item.Id);
            }
            var body = await renderer.RenderPartialToStringAsync("/Views/Email/JobOffers.cshtml", sendOfferEmail);

            mailModel.Body = body;
            mailModel.Subject = "List Offer Of Jobs";
            mailModel.to = email;
            if (await senderService.sendMail(mailModel, "List Of Offers"))
            {
                return EmailSendResult.Success;

            }
            return EmailSendResult.NotSend;

        }


        public Task<CompanyDto> GetCompanyByEmail(string email)
        {
            var company = companyRepository.GetEntities().FirstOrDefault(x => x.Email.Equals(email));
            return Task.FromResult(mapper.Map<CompanyDto>(company));
        }

        public List<OfferDto> GetOffersList()
        {
            var offers = offerRepository.GetEntities()
                 .Include(x => x.Company)
                 .ThenInclude(x => x.City)
                 .Include(x => x.OfferLanguages)
                 .ThenInclude(x => x.Language)
                 .Include(x => x.OfferKnowledges)
                 .ThenInclude(x => x.Knowledge)
                .Include(x => x.OfferJobCategories)
                .ThenInclude(x => x.JobCategory).AsQueryable();
              var offersDto=mapper.Map<List<OfferDto>>(offers.ToList());
            return offersDto;
        }

        public async Task<OfferDto> GetOfferById(int id)
        {
            var jobOffer = mapper.Map<OfferDto>(GetOffersList().FirstOrDefault(offer => offer.Id == id));
            return jobOffer;
        }
        public async Task<List<OfferDto>> GetOffers()
        {
            return mapper.Map<List<OfferDto>>(await offerRepository.GetEntities().ToListAsync());
        }
        public async Task<List<CompanyDto>> GetTopEmployers()
        {
            var companies = await companyRepository.GetEntities().Where(x => x.IsTop).ToListAsync();
            return mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<bool> UpdateCompany(CompanyDto companyDto)
        {
            try
            {
                var anyCompany = companyRepository.GetEntities().AsNoTracking()
                   .FirstOrDefault(x => x.CompanyRegistrationId == companyDto.CompanyRegistrationId);
                if (anyCompany != null)
                {
                    if (anyCompany.CountryCode == companyDto.CountryCode && companyDto.Email != anyCompany.Email)
                    {
                        return false;
                    }
                }

                var company = await companyRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(x => x.Email == companyDto.Email);
                if (company == null)
                    return false;
                var companyNew = mapper.Map<Company>(companyDto);
                companyRepository.UpdateEntity(companyNew);
                await companyRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool GetCompanyRole(ClaimsPrincipal user)
        => user.Claims.Where(a => a.Type.Equals(ClaimTypes.Role)).ToList().Select(x => x.Value.Equals("IsCompany")).FirstOrDefault();

        public async Task<JobOfferViewModel> GetJobOfferViewModel(string email)
        {
            var company = await companyRepository.GetEntities().SingleOrDefaultAsync(x => x.Email.Equals(email));
            var jobOfferViewModel = new JobOfferViewModel()
            {
                CurrencyDtos = mapper.Map<List<CurrencyDto>>(await currencyRepository.GetEntities().ToListAsync()),
                KnowledgeDtos = mapper.Map<List<KnowledgeDto>>(await knowledgeRepository.GetEntities().ToListAsync()),
                LanguageDtos = mapper.Map<List<LanguageDto>>(await languageRepository.GetEntities().ToListAsync()),
                PackageDtos = mapper.Map<List<PackageDto>>(await packageRepository.GetEntities().ToListAsync()),
                CompanyDto = mapper.Map<CompanyDto>(company),
                OfferDto = new OfferDto()
            };
            return jobOfferViewModel;
        }

        public async Task<int> SaveOrUpdateJobOffer(OfferDto offerDto)
        {
            if (offerDto != null)
            {
                if (offerDto.Id == 0)
                {
                    try
                    {
                        var offer = new Offer()
                        {
                            Agreement = offerDto.Agreement,
                            BasicSalary = offerDto.BasicSalary,
                            Benifits = offerDto.Benifits,
                            PackageName = offerDto.PackageName,
                            DoShowApplicantsSalary = offerDto.DoShowApplicantsSalary,
                            Freelancer = offerDto.Freelancer,
                            HasDriverLicenceA = offerDto.HasDriverLicenceA,
                            HasDriverLicenceB = offerDto.HasDriverLicenceB,
                            HasDriverLicenceC = offerDto.HasDriverLicenceC,
                            HasDriverLicenceD = offerDto.HasDriverLicenceD,
                            HasDriverLicenceE = offerDto.HasDriverLicenceE,
                            HasHigherEducationLevel1 = offerDto.HasHigherEducationLevel1,
                            HasHigherEducationLevel2 = offerDto.HasHigherEducationLevel2,
                            HasHigherEducationLevel3 = offerDto.HasHigherEducationLevel3,
                            HasHighSchoolStudent = offerDto.HasHighSchoolStudent,
                            HasPostSecondary = offerDto.HasPostSecondary,
                            HasPrimaryEducation = offerDto.HasPrimaryEducation,
                            HasSecondarySchoolWithGraduation = offerDto.HasSecondarySchoolWithGraduation,
                            HasSecondarySchoolWithOutGraduation = offerDto.HasSecondarySchoolWithOutGraduation,
                            IsFullTime = offerDto.IsFullTime,
                            IsInternShip = offerDto.IsInternShip,
                            HasUniversityStudent = offerDto.HasUniversityStudent,
                            IsPartTime = offerDto.IsPartTime,
                            IsPartialWorkFromHome = offerDto.IsPartialWorkFromHome,
                            IsTravelRequirement = offerDto.IsTravelRequirement,
                            IsWorkAtTheRegularWorkPlace = offerDto.IsWorkAtTheRegularWorkPlace,
                            IsWorkingFromHome = offerDto.IsWorkingFromHome,
                            JobDescription = offerDto.JobDescription,

                            JobTitle = offerDto.JobTitle,
                            PerType = offerDto.PerType,
                            WorkPlaceAddress = offerDto.WorkPlaceAddress,
                            Vat = offerDto.Vat,
                            UpperLimit = offerDto.UpperLimit,
                            CompanyEmail = offerDto.CompanyEmail,
                            RegisterDate = DateTime.Now,
                            CurrencyCode = offerDto.CurrencyCode
                        };
                        offer.Company = null;
                        var package = packageRepository.GetEntities().FirstOrDefault(x => x.PackageName.Equals(offerDto.PackageName));
                        if (package != null)
                        {
                            var currencyRating = currencyRepository.GetEntities()
                                .SingleOrDefault(x => x.Code.Equals(offerDto.CurrencyCode));

                            offer.Price = package.Price * currencyRating.CurrencyRate;
                            offer.ExprationDate = offer.RegisterDate.AddDays(package.DurationInDays);
                            offer.Vat = 0;
                        }
                        await offerRepository.AddEntity(offer);
                        await offerRepository.SaveChange();
                    }
                    catch (System.Exception)
                    {

                        return 0;
                    }
                    var offerId = offerRepository.GetEntities().Max(x => x.Id);

                    if (offerDto.LanguageSkillsOptionals != null)
                    {
                        var ListOptionalLanguage = new List<OfferLanguage>();
                        foreach (var item in offerDto.LanguageSkillsOptionals)
                        {
                            var offerLanguage = new OfferLanguage()
                            {
                                LanguageId = item,
                                LanguageLevel = (int)OfferLanguageType.Optional,
                                OfferId = offerId
                            };
                            ListOptionalLanguage.Add(offerLanguage);
                        }
                        await offerLanguageRepository.AddEntityRange(ListOptionalLanguage);
                    }


                    if (offerDto.LanguageSkillsRequireds != null)
                    {
                        var ListRequiredLanguage = new List<OfferLanguage>();
                        foreach (var item in offerDto.LanguageSkillsRequireds)
                        {
                            var offerLanguage = new OfferLanguage()
                            {
                                LanguageId = item,
                                LanguageLevel = (int)OfferLanguageType.Required,
                                OfferId = offerId
                            };
                            ListRequiredLanguage.Add(offerLanguage);
                        }
                        await offerLanguageRepository.AddEntityRange(ListRequiredLanguage);
                    }

                    if (offerDto.JobCategories != null)
                    {
                        var ListJobCategories = new List<OfferJobCategory>();
                        foreach (var item in offerDto.JobCategories)
                        {
                            var offerJobCategory = new OfferJobCategory()
                            {
                                JobCategoryId = item,
                                OfferId = offerId
                            };
                            ListJobCategories.Add(offerJobCategory);
                        }
                        await offerJobCategoryRepository.AddEntityRange(ListJobCategories);
                        await offerJobCategoryRepository.SaveChange();
                    }

                    if (offerDto.RequiredSeniorKnowledges != null)
                    {
                        var ListRequiredKnowledge = new List<OfferKnowledge>();
                        foreach (var item in offerDto.RequiredSeniorKnowledges)
                        {
                            var offerKnowledge = new OfferKnowledge()
                            {
                                KnowledgeId = item,
                                KnowledgeLevel = (int)OfferKnowledgeType.RequiredKnowledge,
                                OfferId = offerId
                            };
                            ListRequiredKnowledge.Add(offerKnowledge);
                        }
                        await offerKnowledgeRepository.AddEntityRange(ListRequiredKnowledge);
                    }


                    if (offerDto.RequiredJuniorKnowledges != null)
                    {
                        var ListAddvancedRequiredKnowledge = new List<OfferKnowledge>();
                        foreach (var item in offerDto.RequiredJuniorKnowledges)
                        {
                            var offerKnowledge = new OfferKnowledge()
                            {
                                KnowledgeId = item,
                                KnowledgeLevel = (int)OfferKnowledgeType.AddvantageRequiredKnowledge,
                                OfferId = offerId
                            };
                            ListAddvancedRequiredKnowledge.Add(offerKnowledge);
                        }
                        await offerKnowledgeRepository.AddEntityRange(ListAddvancedRequiredKnowledge);
                    }


                    if (offerDto.OptionalKnowledges != null)
                    {
                        var ListAddvancedOptionalKnowledge = new List<OfferKnowledge>();
                        foreach (var item in offerDto.OptionalKnowledges)
                        {
                            var offerKnowledge = new OfferKnowledge()
                            {
                                KnowledgeId = item,
                                KnowledgeLevel = (int)OfferKnowledgeType.AddvantageOptionalKnowledge,
                                OfferId = offerId
                            };
                            ListAddvancedOptionalKnowledge.Add(offerKnowledge);
                        }
                        await offerKnowledgeRepository.AddEntityRange(ListAddvancedOptionalKnowledge);
                        await offerLanguageRepository.SaveChange();
                    }
                    return offerId;
                }
            }
            return 0;
        }
        public PaginationDto<GetOfferListMobile> GetoffersByPagination(OffersFilter offersFilter = null)
        {
            IQueryable<OfferDto> FilteredRecords = GetoffersByFilters(offersFilter);
            List<GetOfferListMobile> model=new List<GetOfferListMobile>();
            foreach (var item in FilteredRecords)
            {
                var offerMobile = new GetOfferListMobile()
                {
                    Id = item.Id,
                    CompanyName = item.CompanyDto.Name ?? "",
                    JobTitle = "JobTitle : " + item.JobTitle ?? "",
                    CompanyEmail = item.CompanyEmail ?? "",
                    Description = item.JobDescription ?? "",
                    City = item.CompanyDto.CityDto.Name ?? "",
                    DateOfOffer = item.RegisterDate,
                    Logo = item.CompanyDto.Logo,
                    JobCategoryNameList = item.OfferJobCategoryDtos.Select(x => x.JobCategoryDto.Jobcategory).ToList()
                };
                model.Add(offerMobile);
            }
            var count = FilteredRecords.Count();
            var skip = Math.Min((offersFilter.pageNumber - 1) * offersFilter.pageSize, count - 1);
            var offersDtos = FilteredRecords.Skip(skip).Take(offersFilter.pageSize).ToList();

            var result = new PaginationDto<GetOfferListMobile>
            {
                Data = model,
                PageCount = (int)Math.Ceiling(((double)count / offersFilter.pageSize)),
                ItemsCount = count
            };
            return result;
        }

        public OffersFilter CalculateOfferDtoFilter(OffersFilter offersFilter)
        {
            IQueryable<OfferDto> FilteredRecords = GetoffersByFilters(offersFilter);
            var offerFilter = GetOffersFilter(FilteredRecords.ToList());
            return offerFilter;
        }

        public IQueryable<OfferDto> GetoffersByFilters(OffersFilter offersFilter = null)
        {
            var filteredModel = new List<OfferDto>();
            var model = GetOffersList().AsQueryable();
            if (!string.IsNullOrWhiteSpace(offersFilter.cityName) && offersFilter.cityName != null)
            {
                model = model.Where(x => x.CompanyDto.CityDto.Name.ToLower().Trim().StartsWith(offersFilter.cityName.ToLower().Trim()));
            }
            if (offersFilter.jobCategoryFilters.Count > 0 && offersFilter.jobCategoryFilters != null)
            {
                foreach (var item in offersFilter.jobCategoryFilters)
                {
                    var NewModel = model.Where(x => x.OfferJobCategoryDtos.Any(s => s.JobCategoryDto.Jobcategory.ToLower().StartsWith(item.CategoryName.ToLower())));
                    filteredModel.AddRange(NewModel);
                    
                }
                model = filteredModel.OrderBy(x => x.Id).Distinct().AsQueryable();
            }
            if (offersFilter.CompanyOfferFilters.Count > 0 && offersFilter.CompanyOfferFilters != null)
            {
                foreach (var item in offersFilter.CompanyOfferFilters)
                {
                    var NewModel = model.Where(x => x.CompanyDto.Name == item.ComapnyName);
                    
                    filteredModel.AddRange(NewModel.ToList());
                }
                model = filteredModel.OrderBy(x=>x.Id).Distinct().AsQueryable();
            }
            if (offersFilter.LanguageSkillFilters.Count > 0 && offersFilter.LanguageSkillFilters != null)
            {
                foreach (var item in offersFilter.LanguageSkillFilters)
                {
                    var NewModel = model.Where(x => x.OfferLanguageDtos.Any(s => s.LanguageDto.Id == item.LanguageSkillId));
                   
                    filteredModel.AddRange(NewModel.ToList());
                }
                model = filteredModel.OrderBy(x=>x.Id).Distinct().AsQueryable();
            }
            if (offersFilter.TypeOfEmployeeFilters.Count > 0 && offersFilter.TypeOfEmployeeFilters != null)
            {
                foreach (var item in offersFilter.TypeOfEmployeeFilters)
                {
                    if (item.Type.Equals(Res.CompanyOffer.FullTime))
                    {
                        var NewModel = model.Where(x => x.IsFullTime);
                       if(NewModel.Count()>0)
                        model = NewModel;

                    }
                    if (item.Type.Equals(Res.CompanyOffer.PartTime))
                    {
                        var NewModel = model.Where(x => x.IsPartTime);
                        if (NewModel.Count() > 0)
                            model = NewModel;
                    }
                    if (item.Type.Equals(Res.CompanyOffer.Freelancer))
                    {
                        var NewModel = model.Where(x => x.Freelancer);
                        if (NewModel.Count() > 0)
                            model = NewModel;
                    }
                    if (item.Type.Equals(Res.CompanyOffer.Agreement))
                    {
                        var NewModel = model.Where(x => x.Agreement);
                        if (NewModel.Count() > 0)
                            model = NewModel;
                    }
                    if (item.Type.Equals(Res.CompanyOffer.InternShip))
                    {
                        var NewModel = model.Where(x => x.IsInternShip);
                        if (NewModel.Count() > 0)
                            model = NewModel;
                    }
                }
                model = filteredModel.OrderBy(x=>x.Id).Distinct().AsQueryable();
            }
            if (offersFilter.WorkAreaFilters.Count > 0 && offersFilter.WorkAreaFilters != null)
            {
                foreach (var item in offersFilter.WorkAreaFilters)
                {
                    if (item.WorkAreaName.Equals(Res.CompanyOffer.WorkingFromHome))
                    {
                        var NewModel = model.Where(x => x.IsWorkingFromHome);
                        if (NewModel.Count() > 0)
                            filteredModel.AddRange(NewModel);
                    }
                    if (item.WorkAreaName.Equals(Res.CompanyOffer.PartialWorkFromHome))
                    {
                        var NewModel = model.Where(x => x.IsPartialWorkFromHome);
                        if (NewModel.Count() > 0)
                            filteredModel.AddRange(NewModel);
                    }
                    if (item.WorkAreaName.Equals(Res.CompanyOffer.WorkAtTheRegularWorkPlace))
                    {
                        var NewModel = model.Where(x => x.IsWorkAtTheRegularWorkPlace);
                        if (NewModel.Count() > 0)
                            filteredModel.AddRange(NewModel);
                    }
                }
                model = filteredModel.OrderBy(x=>x.Id).Distinct().AsQueryable();
            }
            return model;
        }
        public OffersFilter GetOffersFilter(List<OfferDto> offerDtos = null)
        {
            List<OfferDto> model;
            if (offerDtos == null)
            {
                model = GetOffersList();
            }
            else
            {
                model = offerDtos;
            }


            List<WorkAreaFilter> workAreaFilters = new List<WorkAreaFilter>
            {
                new WorkAreaFilter(){
                WorkAreaName = Res.CompanyOffer.WorkingFromHome,
                Count = model.Where(x => x.IsWorkingFromHome).Count()
            },
            new WorkAreaFilter()
            {
                WorkAreaName = Res.CompanyOffer.PartialWorkFromHome,
                Count = model.Where(x => x.IsPartialWorkFromHome).Count()
            },
            new WorkAreaFilter()
            {
                WorkAreaName = Res.CompanyOffer.WorkAtTheRegularWorkPlace,
                Count = model.Where(x => x.IsWorkAtTheRegularWorkPlace).Count()
            }
            };
            List<TypeOfEmployeeFilter> typeOfEmployeeFilters = new List<TypeOfEmployeeFilter>
            {
                new TypeOfEmployeeFilter()
                {
                    Type=Res.CompanyOffer.FullTime,
                    Count=model.Where(x=>x.IsFullTime).Count()
                },
                new TypeOfEmployeeFilter()
                {
                    Type = Res.CompanyOffer.PartTime,
                    Count = model.Where(model => model.IsPartTime).Count()
                },
                new TypeOfEmployeeFilter()
                {
                    Type=Res.CompanyOffer.Agreement,
                    Count=model.Where(x=>x.Agreement).Count()
                },
                  new TypeOfEmployeeFilter()
                  {
                      Type=Res.CompanyOffer.Freelancer,
                      Count=model.Where(x=>x.Freelancer).Count()
                  },
                  new TypeOfEmployeeFilter()
                  {
                      Type=Res.CompanyOffer.InternShip,
                      Count=model.Where(x=>x.IsInternShip).Count()
                  }

            };
            List<JobOfferExpireTimeFilter> jobOfferExpireTimeFilters = new List<JobOfferExpireTimeFilter>();
            foreach (var item in packageRepository.GetEntities())
            {
                var packageName = "";
                if (item.PackageName.ToLower().Contains("1 week")) packageName = Res.CompanyOffer.ForOneWeek;
                else if (item.PackageName.ToLower().Contains("2 week")) packageName = Res.CompanyOffer.ForTwoWeeks;
                else if (item.PackageName.ToLower().Contains("1 month")) packageName = Res.CompanyOffer.ForOneMonth;
                else if (item.PackageName.ToLower().Contains("2 month")) packageName = Res.CompanyOffer.ForTwoMonths;
                var package = new JobOfferExpireTimeFilter { JobOfferTimeName = packageName, Count = model.Where(x => x.PackageName.Equals(item.PackageName)).Count() };

                jobOfferExpireTimeFilters.Add(package);
            }
            List<CompanyOfferFilter> companyOfferFilters = new List<CompanyOfferFilter>();
            var companis = model.Select(x => x.CompanyDto.Name).Distinct().ToList();
            foreach (var item in companis)
            {
                companyOfferFilters.Add(new CompanyOfferFilter { ComapnyName = item, Count = model.Where(x => x.CompanyDto.Name.Equals(item)).Count() });
            }

            List<JobCategoryFilter> jobCategoryFilters = new List<JobCategoryFilter>();
            var jobCategoriIdList = model.SelectMany(x => x.OfferJobCategoryDtos).Select(s => s.JobCategoryDto.Id).Distinct();


            foreach (var item in jobCategoriIdList)
            {
                var job = new JobCategoryFilter
                {
                    CategoryId = item,
                    CategoryName = jobCategoryRepository.GetEntities().SingleOrDefault(x => x.Id == item).Jobcategory,
                    Count = jobCategoryCount(item),
                };
                if (job.Count != 0)
                {
                    jobCategoryFilters.Add(job);
                }


            }

            List<LanguageSkillFilter> languageSkillFilters = new List<LanguageSkillFilter>();
            var languageSkilIdList = model.SelectMany(x => x.OfferLanguageDtos).Select(s => s.LanguageDto.Id).Distinct();

            foreach (var item in languageSkilIdList)
            {
                languageSkillFilters.Add(new LanguageSkillFilter
                {
                    LanguageSkillId = item,
                    LanguageName = languageRepository.GetEntities().SingleOrDefault(x => x.Id == item).Name,
                    Count = LanguageSkillCount(item)
                });
            }



            var offerFilter = new OffersFilter()
            {
                WorkAreaFilters = workAreaFilters,
                TypeOfEmployeeFilters = typeOfEmployeeFilters,
                CompanyOfferFilters = companyOfferFilters,
                jobCategoryFilters = jobCategoryFilters,
                LanguageSkillFilters = languageSkillFilters,
                JobOfferExpireTimeFilters = jobOfferExpireTimeFilters,
            };
            offerFilter.cityName = "";
            return offerFilter;
        }

        



        private int jobCategoryCount(int id)
        {
            int sum = 0;
            var model = GetOffersList().SelectMany(x => x.OfferJobCategoryDtos.Select(s => s.JobCategoryDto));

            foreach (var item in model)
            {


                if (item.Id == id) sum++;


            }
            return sum;
        }
        private int LanguageSkillCount(int id)
        {
            int sum = 0;
            var model = GetOffersList().SelectMany(x => x.OfferLanguageDtos.Select(s => s.LanguageDto));

            foreach (var item in model)
            {


                if (item.Id == id) sum++;

            }
            return sum;
        }

    }

}

