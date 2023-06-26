using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly ICitiesService citiesService;

        private readonly ICountryService countryService;
        private readonly IGenericRepository<ApplicantDocuments> applicantDocumentRepository;
        private readonly IGenericRepository<ApplicantWorkExperience> workExperienceRepository;
        private readonly IGenericRepository<ApplicantEducation> educationRepository;
        private readonly IGenericRepository<ApplicantKnowledge> applicantknowledgeRepository;
        private readonly IGenericRepository<ApplicantLanguage> applicantlanguageRepository;
        private readonly IGenericRepository<Applicant> profileRepository;
        private readonly IGenericRepository<Language> languageRepository;
        private readonly IGenericRepository<Knowledge> knowledgeRepository;
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IGenericRepositorySimple<ApplicantPreference> preferenceRepository;
        private readonly IGenericRepository<OfferJobCategory> offerJobCategoryRepository;
        private readonly IGenericRepository<ApplicantOffersFavourite> applicantOfferFavouritRepository;
        private readonly IJobCategoriService jobCategoriService;

        private readonly IMapper mapper;

        public ApplicantService(IMapper mapper
            , ICitiesService citiesService, ICountryService countryService,
            IGenericRepository<ApplicantDocuments> applicantDocumentRepository,
            IGenericRepository<ApplicantWorkExperience> workExperienceRepository,
            IGenericRepository<ApplicantEducation> educationRepository,
            IGenericRepository<ApplicantKnowledge> ApplicantknowledgeRepository,
            IGenericRepository<ApplicantLanguage> ApplicantlanguageRepository,
            IGenericRepository<Applicant> ProfileRepository,
            IGenericRepository<Language> LanguageRepository,
            IGenericRepository<Knowledge> KnowledgeRepository,
            IGenericRepository<Offer> offerRepository,
            IGenericRepositorySimple<ApplicantPreference> preferenceRepository,
            IGenericRepository<OfferJobCategory> offerJobCategoryRepository,
            IGenericRepository<ApplicantOffersFavourite> ApplicantOfferFavouritRepository,
            IJobCategoriService jobCategoriService)

        {
            this.countryService = countryService;
            this.applicantDocumentRepository = applicantDocumentRepository;
            this.workExperienceRepository = workExperienceRepository;
            this.educationRepository = educationRepository;
            applicantknowledgeRepository = ApplicantknowledgeRepository;
            applicantlanguageRepository = ApplicantlanguageRepository;
            this.profileRepository = ProfileRepository;
            languageRepository = LanguageRepository;
            knowledgeRepository = KnowledgeRepository;
            this.offerRepository = offerRepository;
            this.mapper = mapper;
            this.citiesService = citiesService;
            this.preferenceRepository = preferenceRepository;
            this.offerJobCategoryRepository = offerJobCategoryRepository;
           this.applicantOfferFavouritRepository = ApplicantOfferFavouritRepository;
            this.jobCategoriService = jobCategoriService;
        }

        #region Claims
        public bool GetApplicantUserClaims(ClaimsPrincipal user)
        {
            return user.Claims.Where(a => a.Type.Equals(ClaimTypes.Role)).ToList().Select(x => x.Value.Equals("IsApplicant")).FirstOrDefault();
        }
        #endregion
        #region Applicant
        public ApplicantDto GetApplicant(string email)
        {
            var applicant = GetApplicantsQuery().FirstOrDefault(a => a.Email.Equals(email));
            return mapper.Map<ApplicantDto>(applicant);
        }
        public async Task<bool> IsApplicantExist(string email)
        {
            var applicant = await profileRepository.GetEntities().SingleOrDefaultAsync(x => x.Email.Equals(email));
            if (applicant is null)
                return false;
            return true;
        }

        public ApplicantDto GetApplicant(int id)
        {
            var applicant = GetApplicantsQuery().FirstOrDefault(a => a.Id.Equals(id));
            return mapper.Map<ApplicantDto>(applicant);
        }

        public List<ApplicantDto> GetApplicants(int skip, int take)
        {
            var applicants = GetApplicantsQuery().Skip(skip).Take(take);
            return mapper.Map<List<ApplicantDto>>(applicants);
        }
        public PaginationDto<ApplicantDto> GetApplicants(int currentPage, string? key, string? location)
        {
            var itemPerPage = 5;

            var query = profileRepository.GetEntities().Include(x => x.ApplicantKnowledges).AsQueryable();
            if (!string.IsNullOrWhiteSpace(key))
                query = query.Where(x => x.JobPosition.Contains(key));
            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(x => x.Country.Name.Contains(location) || x.CityName.Contains(location));
            var count = query.Count();
            var skip = Math.Min((currentPage - 1) * itemPerPage, count - 1);
            query = query.Skip(skip).Take(itemPerPage);
            try
            {
                var data = mapper.Map<List<ApplicantDto>>(query);
                var result = new PaginationDto<ApplicantDto>
                {
                    Data = data,
                    PageCount = (int)Math.Ceiling(((double)count / itemPerPage)),
                    ItemsCount = count
                };
                return result;
            }
            catch
            {
                return new PaginationDto<ApplicantDto>();
            }
        }
        public int GetApplicantsCount(string key, string location)
        {
            var query = profileRepository.GetEntities().Include(x => x.ApplicantKnowledges).AsQueryable();
            if (!string.IsNullOrWhiteSpace(key))
                query = query.Where(x => x.JobPosition.Contains(key));
            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(x => x.Country.Name.Contains(location) || x.CityName.Contains(location));
            return query.Count();
        }

        private IQueryable<Applicant> GetApplicantsQuery()
        {
            var applicantsQuery = profileRepository.GetEntities()
                  .Include(a => a.ApplicantEducations)
                  .Include(a => a.ApplicantKnowledges)
                  .Include(a => a.ApplicantLanguages)
                  .Include(a => a.ApplicantWorkExperiences)
                  .Include(a => a.ApplicantDocuments)
                  .AsNoTracking();
            return applicantsQuery;
        }
        #endregion
        #region Applicant Image
        public async Task<bool> UpdateApplicantImage(string imageBase64, string applicantEmail)
        {
            var applicant = await profileRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(a => a.Email.Equals(applicantEmail));
            applicant.ApplicantImage = imageBase64;
            try
            {
                profileRepository.UpdateEntity(applicant);
                await profileRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion
        #region ApplicantDocuments
        public List<ApplicantDocumentsDto> GetApplicantDocumentsByEmail(string email)
        {
            var ApplicantDocumentsList = applicantDocumentRepository.GetEntities().AsNoTracking().Where(x => x.ApplicantEmail.Equals(email));
            if (ApplicantDocumentsList == null)
                return null;
            var ApplicantDocumentsListDtoList = mapper.Map<List<ApplicantDocumentsDto>>(ApplicantDocumentsList);
            return ApplicantDocumentsListDtoList;
        }
        public async Task<bool> DeleteApplicantDocument(int id)
        {
            var applicant = await applicantDocumentRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (applicant == null)
                return false;
            try
            {
                applicantDocumentRepository.DeleteEntity(applicant);
                await applicantDocumentRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<bool> InsertApplicantDocument(ApplicantDocumentsDto applicantDocumentsDto)
        {
            try
            {
                var documents = applicantDocumentRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(applicantDocumentsDto.ApplicantEmail));
                if (documents.Count() >= 0 && documents.Count() < 6)
                {
                    var applicantDocument = mapper.Map<ApplicantDocuments>(applicantDocumentsDto);
                    await applicantDocumentRepository.AddEntity(applicantDocument);
                    await applicantDocumentRepository.SaveChange();
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateApplicantDocument(ApplicantDocumentsDto applicantDocumentsDto)
        {
            try
            {
                var applicantDocument = applicantDocumentRepository.GetEntities().AsNoTracking().SingleOrDefault(x => x.Id == applicantDocumentsDto.Id);
                if (applicantDocument == null)
                    return false;
                var ApplicantDocument = mapper.Map<ApplicantDocuments>(applicantDocumentsDto);
                applicantDocumentRepository.UpdateEntity(applicantDocument);
                await applicantDocumentRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        #endregion
        #region WorkExperience
        public List<ApplicantWorkExperienceDto> GetAllApplicantWorkExperienceByEmail(string email)
        {
            var workExpreienceList = workExperienceRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(email));
            if (workExpreienceList == null)
                return null;
            var workExperienceListDto = mapper.Map<List<ApplicantWorkExperienceDto>>(workExpreienceList);
            return workExperienceListDto;
        }
        public async Task<bool> InsertOrUpdateWorkExperiance(ApplicantWorkExperienceDto workExperienceDto)
        {
            try
            {
                if (workExperienceDto.Id != 0)
                {
                    var workExperience = await workExperienceRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(x => x.Id == workExperienceDto.Id);
                    if (workExperience == null)
                        return false;
                    var workExperienceNew = mapper.Map<ApplicantWorkExperience>(workExperienceDto);
                    workExperienceRepository.UpdateEntity(workExperienceNew);
                    await workExperienceRepository.SaveChange();
                    return true;
                }
                else
                {
                    var workExperiencNew = mapper.Map<ApplicantWorkExperience>(workExperienceDto);
                    await workExperienceRepository.AddEntity(workExperiencNew);
                    await workExperienceRepository.SaveChange();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<ApplicantWorkExperienceDto> GetWorkExperienceById(int id)
        {
            var workExperience = await workExperienceRepository.GetEntities().SingleOrDefaultAsync(i => i.Id == id);
            if (workExperience == null)
                return null;
            return mapper.Map<ApplicantWorkExperienceDto>(workExperience);
        }

        public async Task<bool> DeleteApplicantWorkExperience(int id)
        {
            var applicant = await workExperienceRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == id);
            if (applicant == null)
                return false;
            try
            {
                workExperienceRepository.DeleteEntity(applicant);
                await workExperienceRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #endregion
        #region Applicant Education
        public async Task<bool> CreateOrUpdateEducation(ApplicantEducationDto educationDto)
        {
            try
            {
                if (educationDto.Id != 0)
                {
                    var ApplicantEducation = await educationRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(x => x.Id == educationDto.Id);
                    if (ApplicantEducation == null)
                        return false;
                    var ApplicantEducationNew = mapper.Map<ApplicantEducation>(educationDto);
                    educationRepository.UpdateEntity(ApplicantEducationNew);
                    await educationRepository.SaveChange();
                    return true;
                }
                else
                {
                    var ApplicantEducationNew = mapper.Map<ApplicantEducation>(educationDto);
                    await educationRepository.AddEntity(ApplicantEducationNew);
                    await educationRepository.SaveChange();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<ApplicantEducationDto> GetAllApplicantEducation(string email)
        {
            var ApplicantEducationList = educationRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(email));
            if (ApplicantEducationList == null)
                return null;
            var ApplicantEducationListDto = mapper.Map<List<ApplicantEducationDto>>(ApplicantEducationList);
            return ApplicantEducationListDto;
        }

        public async Task<ApplicantEducationDto> GetApplicantEducationById(int id)
        {
            var applicantEducation = await educationRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
            if (applicantEducation == null)
                return null;
            return mapper.Map<ApplicantEducationDto>(applicantEducation);
        }

        public async Task<bool> DeleteApplicantEducation(int id)
        {
            var applicantEducation = await educationRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (applicantEducation == null)
                return false;
            try
            {
                educationRepository.DeleteEntity(applicantEducation);
                await educationRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        #region Applicant Knowledge

        public async Task<bool> CreateOrUpdateKhnowledge(ApplicantKnowledgeDto knowledgeDto)
        {
            try
            {
                if (knowledgeDto.Id != 0)
                {
                    var ApplicantKnowledge = await applicantknowledgeRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(x => x.Id == knowledgeDto.Id);
                    if (ApplicantKnowledge == null)
                        return false;
                    var ApplicantKnowledgeNew = mapper.Map<ApplicantKnowledge>(knowledgeDto);
                    applicantknowledgeRepository.UpdateEntity(ApplicantKnowledgeNew);
                    await applicantknowledgeRepository.SaveChange();
                    return true;
                }
                else
                {
                    var ApplicantKnowledgeNew = mapper.Map<ApplicantKnowledge>(knowledgeDto);
                    await applicantknowledgeRepository.AddEntity(ApplicantKnowledgeNew);
                    await applicantknowledgeRepository.SaveChange();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<ApplicantKnowledgeDto> GetAllApplicantKnowledge(string email)
        {
            var ApplicantKnowledgeList = applicantknowledgeRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(email)).AsNoTracking();
            if (ApplicantKnowledgeList == null)
                return null;
            var ApplicantKnowledgeListDto = mapper.Map<List<ApplicantKnowledgeDto>>(ApplicantKnowledgeList);
            return ApplicantKnowledgeListDto;
        }
        public List<KnowledgeDto> GetKnowledgeList()
        {
            var knowledgeList = knowledgeRepository.GetEntities().ToList();
            var knowledgeDtoList = mapper.Map<List<KnowledgeDto>>(knowledgeList);
            return knowledgeDtoList;
        }

        public async Task<ApplicantKnowledgeDto> GetApplicantKnowledgeById(int id)
        {
            var applicantKnowledge = await applicantknowledgeRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
            if (applicantKnowledge == null)
                return null;
            return mapper.Map<ApplicantKnowledgeDto>(applicantKnowledge);
        }

        public async Task<bool> DeleteApplicantKnowledge(int id)
        {
            var applicantKnowledge = await applicantknowledgeRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (applicantKnowledge == null)
                return false;
            try
            {
                applicantknowledgeRepository.DeleteEntity(applicantKnowledge);
                await applicantknowledgeRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        #region Applicant Language
        public async Task<bool> CreateOrUpdateLanguage(ApplicantLanguageDto languageDto)
        {
            try
            {
                if (languageDto.Id != 0)
                {
                    var applicantLanguage = await applicantlanguageRepository.GetEntities().AsNoTracking().SingleOrDefaultAsync(x => x.Id == languageDto.Id);
                    if (applicantLanguage == null)
                        return false;
                    var applicantLanguageNew = mapper.Map<ApplicantLanguage>(languageDto);
                    applicantlanguageRepository.UpdateEntity(applicantLanguageNew);
                    await applicantlanguageRepository.SaveChange();
                    return true;
                }
                else
                {
                    var applicantLanguageNew = mapper.Map<ApplicantLanguage>(languageDto);
                    await applicantlanguageRepository.AddEntity(applicantLanguageNew);
                    await applicantlanguageRepository.SaveChange();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<ApplicantLanguageDto> GetAllApplicantLanguage(string email)
        {
            var ApplicantLanguageList = applicantlanguageRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(email));
            if (ApplicantLanguageList == null)
                return null;
            var ApplicantLanguageListDto = mapper.Map<List<ApplicantLanguageDto>>(ApplicantLanguageList);
            return ApplicantLanguageListDto;
        }

        public async Task<ApplicantLanguageDto> GetApplicantLanguageById(int id)
        {
            var applicantLanguage = await applicantlanguageRepository.GetEntities().SingleOrDefaultAsync(i => i.Id == id);
            if (applicantLanguage == null)
                return null;
            return mapper.Map<ApplicantLanguageDto>(applicantLanguage);
        }

        public async Task<bool> DeleteApplicantLanguage(int id)
        {
            var applicantLanguage = await applicantlanguageRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == id);
            if (applicantLanguage == null)
                return false;
            try
            {
                applicantlanguageRepository.DeleteEntity(applicantLanguage);
                await applicantlanguageRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public List<LanguageDto> GetLanguageList()
        {
            var languages = languageRepository.GetEntities().ToList();
            var knowledgeDtos = mapper.Map<List<LanguageDto>>(languages);
            return knowledgeDtos;
        }


        #endregion
        #region Personal Information
        public ApplicantDto GetPersonalInformation(string email)
        {
            var applicant = profileRepository.GetEntities().SingleOrDefault(x => x.Email.Equals(email));
            var PersonalInformationDto = mapper.Map<ApplicantDto>(applicant);
            return PersonalInformationDto;
        }

        public async Task<bool> SavePersonalInformation(ApplicantProfileDto personalInformation)
        {
            var applicant = await profileRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Email == personalInformation.Email);
            if (applicant == null)
                return false;
            try
            {
                applicant.FirstName = personalInformation.FirstName;
                applicant.LastName = personalInformation.LastName;
                applicant.Gender = personalInformation.Gender;
                applicant.DateOfBirth = personalInformation.DateOfBirth;
                applicant.AvailableDate = personalInformation.AvailableDate;
                applicant.ReadyToWorkStatus = personalInformation.ReadyToWorkStatus;
                applicant.JobPosition = personalInformation.JobPosition;
                profileRepository.UpdateEntity(applicant);
                await profileRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Contact Detail
        public ApplicantDto GetApplicantContactDetail(string email)
        {
            var applicant = profileRepository.GetEntities().AsNoTracking().FirstOrDefault(x => x.Email == email);
            var PersonalContactDetailDto = mapper.Map<ApplicantDto>(applicant);
            return PersonalContactDetailDto;
        }



        public async Task<bool> SaveApplicantContactDetail(ApplicantContactDetailsDto contactDetailsDto, string email)
        {

            var applicant = await profileRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);



            if (applicant == null)
                return false;
            try
            {
                applicant.CountryCode = contactDetailsDto.CountryCode;
                applicant.CityId = contactDetailsDto.CityId;
                applicant.Phone = contactDetailsDto.Phone;
                applicant.Address = contactDetailsDto.Address;
                applicant.PostalCode = contactDetailsDto.PostalCode;
                applicant.CityName = contactDetailsDto.CityName;
                applicant.StateName = contactDetailsDto.StateName;

                profileRepository.UpdateEntity(applicant);
                await profileRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Applicant Additional Section
        public ApplicantDto GetApplicantAdditionalSection(string email)
        {
            var applicant = profileRepository.GetEntities().FirstOrDefault(x => x.Email == email);

            var applicantAdditionalSection = mapper.Map<ApplicantDto>(applicant);
            applicantAdditionalSection.Email = email;
            return applicantAdditionalSection;
        }

        public async Task<bool> SaveApplicantAdditionalSection(ApplicantAddtionalSectionDto addtionalSectionDto)
        {
            var applicant = await profileRepository.GetEntities().AsNoTracking().FirstOrDefaultAsync(x => x.Email == addtionalSectionDto.Email);
            if (applicant == null)
                return false;
            try
            {
                applicant.IsDrivingLicense = addtionalSectionDto.IsDrivingLicense;
                applicant.LicenseType = addtionalSectionDto.LicenseType;
                applicant.IsEuropeanUnion = addtionalSectionDto.IsEuropeanUnion;
                applicant.IsSwitzerland = addtionalSectionDto.IsSwitzerland;
                applicant.RateType = addtionalSectionDto.RateType;
                applicant.IsUnitedStatesofAmerica = addtionalSectionDto.IsUnitedStatesOfAmerica;
                applicant.IsHourlyRate = addtionalSectionDto.IsHourlyRate;
                applicant.HourlyAverage = addtionalSectionDto.HourlyAverage;
                applicant.HourlyFrom = addtionalSectionDto.HourlyFrom;
                applicant.HourlyUntil = addtionalSectionDto.HourlyUntil;
                applicant.IsWorkPlace = addtionalSectionDto.IsWorkPlace;
                applicant.IsWorkFromHome = addtionalSectionDto.IsWorkFromHome;
                applicant.IsPartialworkfromhome = addtionalSectionDto.IsPartialWorkFromHome;
                applicant.IsFullTime = addtionalSectionDto.IsFullTime;
                applicant.IsPartTime = addtionalSectionDto.IsPartTime;
                applicant.IsFreelancer = addtionalSectionDto.IsFreelancer;
                applicant.IsInternShip = addtionalSectionDto.IsInternShip;
                applicant.Currency = addtionalSectionDto.Currency;

                profileRepository.UpdateEntity(applicant);
                await profileRepository.SaveChange();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion
        #region Application Privacy
        public Task<ApplicantPrivacyDto> GetPrivacyByEmail(string email)
        {
            var privacy = profileRepository.GetEntities().FirstOrDefault(x => x.Email.Equals(email));
            return Task.FromResult(mapper.Map<ApplicantPrivacyDto>(privacy));
        }

        async Task<bool> IApplicantService.UpdatePrivacy(ApplicantPrivacyDto applicantPrivacyDto)
        {
            var userPrivacy = GetApplicantsQuery().FirstOrDefault(a => a.Email.Equals(applicantPrivacyDto.Email));
            ;
            userPrivacy.ShowGender = applicantPrivacyDto.ShowGender;
            userPrivacy.ShowAddress = applicantPrivacyDto.ShowAddress;
            userPrivacy.ShowAge = applicantPrivacyDto.ShowAge;
            userPrivacy.ShowCountryOrCity = applicantPrivacyDto.ShowCountryOrCity;
            userPrivacy.AllowSearchEngines = applicantPrivacyDto.AllowSearchEngines;
            userPrivacy.ProfileVisible = applicantPrivacyDto.ProfileVisible;
            userPrivacy.ShowPhone = applicantPrivacyDto.ShowPhone;
            profileRepository.UpdateEntity(userPrivacy);
            await profileRepository.SaveChange();
            return true;
        }
        #endregion
        #region Applicant EmailPreferences

        public async Task<bool> UpdateEmailPreferences(ApplicantPreferenceDto model)
        {
            var records = preferenceRepository.GetEntities().Where(x =>
                x.ApplicantEmail.Equals(model.ApplicantEmail) && x.Category.Equals(model.Category)).ToList();
            if (records.Count > 0)
                foreach (var applicantPreference in records.Where(applicantPreference =>
                             applicantPreference.Category.Equals(model.Category)))
                    applicantPreference.IsSubscribed = model.IsSubscribed;
            else
            {
                var applicantPreference = mapper.Map<ApplicantPreference>(model);

                await preferenceRepository.AddEntity(applicantPreference);
            }


            preferenceRepository.SaveChange();
            return true;
        }

        public List<ApplicantPreferenceDto> GetEmailPreferences(string email)
        {
            var result = preferenceRepository.GetEntities().Where(x => x.ApplicantEmail.Equals(email)).ToList();
            return mapper.Map<List<ApplicantPreferenceDto>>(result);

        }
        public bool UnsubscribeEmailPreferences(string email)
        {
            var records = preferenceRepository.GetEntities().Where(x =>
                x.ApplicantEmail.Equals(email)).ToList();
            if (records.Count > 0)
            {
                foreach (var applicantPreference in records)
                {
                    applicantPreference.IsSubscribed = false;
                    preferenceRepository.UpdateEntity(applicantPreference);
                    preferenceRepository.SaveChange();
                }
            }
            return true;
        }

        #endregion

        #region offers
        public List<OfferDto> GetOffers()
        {
            var offers = mapper.Map<List<OfferDto>>(offerRepository.GetEntities()
                 .Include(x => x.Company)
                 .ThenInclude(x => x.City)
                 .Include(x => x.OfferLanguages)
                 .ThenInclude(x => x.Language)
                 .Include(x => x.OfferKnowledges)
                 .ThenInclude(x => x.Knowledge)
                .Include(x => x.OfferJobCategories)
                .ThenInclude(x => x.JobCategory));

            return offers;
        }

        public PaginationDto<OfferDto> SearchOffersAjax(string location, int currentPage, string language,
            string workPlace, string jobOffer, string employer, string position, string company,
            List<string> jobCategories)
        {
            var itemPerPage = 5;
            var offers = GetOffers();
            List<OfferDto> newOffer = new List<OfferDto>();
            if (jobCategories.First() != null && jobCategories.First().Contains(","))
                jobCategories = jobCategories.First().Split(",").ToList();
            if (jobCategories.Count >= 1 && jobCategories.First() != null)
            {
                foreach (var item in jobCategories)
                {
                    if (item != null)
                    {
                        List<int> offerIds = offerJobCategoryRepository.GetEntities()
                            .Where(x => x.JobCategoryId == Int32.Parse(item)).Select(x => x.OfferId).ToList();

                        foreach (var offerId in offerIds)
                        {
                            var IsOfferExist = newOffer.Any(x => x.Id == offerId);
                            if (!IsOfferExist)
                            {
                                newOffer.AddRange(offers.Where(x => x.Id == offerId).ToList());
                            }
                        }
                    }
                }
                offers = newOffer;

            }
            if (!string.IsNullOrWhiteSpace(location))
            {

                offers = offers.Where(x => x.CompanyDto.CityDto.Name.Contains(location) || x.CompanyDto.CityDto.StateName == location).ToList();
            }

            if (!string.IsNullOrWhiteSpace(company))
            {
                newOffer.Clear();
                newOffer.AddRange(offers.Where(x => x.CompanyDto.Name == company).ToList());
                offers = newOffer;
            }
            if (!string.IsNullOrWhiteSpace(language))
            {
                var requestedLanguage = languageRepository.GetEntities().AsNoTracking().Single(x => x.Name == language);
                foreach (var item in offers)
                {
                    if (item.OfferLanguageDtos.Any(x => x.LanguageDto.Id == requestedLanguage.Id))
                    {
                        newOffer.AddRange(offers.Where(x => x.Id == item.Id));
                    }
                }

                offers = newOffer;
            }
            if (!string.IsNullOrWhiteSpace(jobOffer))
            {
                offers = offers.Where(x => x.PackageName.Equals(jobOffer)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(employer))
            {
                offers = SearchEmployerStatus(employer, offers);
            }
            if (!string.IsNullOrWhiteSpace(workPlace))
            {
                offers = SearchWorkPlace(workPlace, offers);
            }
            if (!string.IsNullOrWhiteSpace(position))
            {
                offers = SearchForJobPostions(position, offers, newOffer);
            }
            var count = offers.Count();
            var skip = Math.Min((currentPage - 1) * itemPerPage, count - 1);
            offers = offers.Skip(skip).Take(itemPerPage).ToList();
            try
            {
                var data = mapper.Map<List<OfferDto>>(offers);
                var result = new PaginationDto<OfferDto>
                {
                    Data = data,
                    PageCount = (int)Math.Ceiling(((double)count / itemPerPage)),
                    ItemsCount = count
                };
                return result;
            }
            catch
            {
                return new PaginationDto<OfferDto>();
            }

        }

        private List<OfferDto> SearchForJobPostions(string position, List<OfferDto> offers, List<OfferDto> newOffer)
        {
            newOffer.Clear();
            var allJobs = jobCategoriService.GetJobCategories().Result.FirstOrDefault(x => x.Jobcategory == position);
            foreach (var item in offers)
            {
                if (item.OfferJobCategoryDtos.Any(x => x.JobCategoryDto.Jobcategory == position))
                {
                    newOffer.AddRange(offers.Where(x => x.Id == item.Id));
                }

            }
            offers = newOffer;
            return offers;
        }

        private static List<OfferDto> SearchWorkPlace(string workPlace, List<OfferDto> offers)
        {
            if (Res.CompanyOffer.WorkOnlyFromHome == workPlace)
            {
                offers = offers.Where(x => x.IsWorkingFromHome).ToList();
            }
            if (Res.CompanyOffer.MaybePartially == workPlace)
            {
                offers = offers.Where(x => x.IsPartialWorkFromHome).ToList();
            }
            if (Res.CompanyOffer.WorkAtTheRegularWorkPlace == workPlace)
            {
                offers = offers.Where(x => x.IsWorkAtTheRegularWorkPlace).ToList();
            }

            return offers;
        }

        private static List<OfferDto> SearchEmployerStatus(string employer, List<OfferDto> offers)
        {
            if (employer.Equals(Res.CompanyOffer.FullTime))
            {
                offers = offers.Where(x => x.IsFullTime).ToList();
            }
            if (employer.Equals(Res.CompanyOffer.PartTime))
            {
                offers = offers.Where(x => x.IsPartTime).ToList();
            }
            if (employer.Equals(Res.CompanyOffer.Agreement))
            {
                offers = offers.Where(x => x.Agreement).ToList();
            }
            if (employer.Equals(Res.CompanyOffer.Freelancer))
            {
                offers = offers.Where(x => x.Freelancer).ToList();
            }
            if (employer.Equals(Res.CompanyOffer.InternShip))
            {
                offers = offers.Where(x => x.IsInternShip).ToList();
            }

            return offers;
        }

        public PaginationDto<OfferDto> SearchOffersAjax()
        {
            var itemPerPage = 5;
            var offers = GetOffers();
            var count = offers.Count();
            offers = offers.ToList();

            try
            {
                var data = mapper.Map<List<OfferDto>>(offers);
                var result = new PaginationDto<OfferDto>
                {
                    Data = data,
                    PageCount = (int)Math.Ceiling(((double)count / itemPerPage)),
                    ItemsCount = count
                };
                return result;
            }
            catch
            {
                return new PaginationDto<OfferDto>();
            }

        }

        public async Task<ApplicantOffersFavouriteResult> SaveApplicantOfferFavourite(ApplicantOffersFavouriteDto applicantOffersFavouriteDto)
        {

            if (applicantOffersFavouriteDto.OfferId == 0) return ApplicantOffersFavouriteResult.Badrequest;
            if (applicantOffersFavouriteDto.ApplicantEmail == null)return ApplicantOffersFavouriteResult.Badrequest;
            if (await IsOfferExistToOfferFavouriteList(applicantOffersFavouriteDto)==false)return ApplicantOffersFavouriteResult.AddedBefore;
            var applicantOfferFavourite = mapper.Map<ApplicantOffersFavourite>(applicantOffersFavouriteDto);

            await applicantOfferFavouritRepository.AddEntity(applicantOfferFavourite);
            await applicantOfferFavouritRepository.SaveChange();
            return ApplicantOffersFavouriteResult.Success;
        }

        private async Task<bool> IsOfferExistToOfferFavouriteList(ApplicantOffersFavouriteDto applicantOffersFavouriteDto)
        {
            var offerFavourite =await applicantOfferFavouritRepository.GetEntities().SingleOrDefaultAsync(x => x.OfferId 
            ==(int) applicantOffersFavouriteDto.OfferId && x.ApplicantEmail== applicantOffersFavouriteDto.ApplicantEmail);
            if (offerFavourite == null) return true;

            return false;
        }

        public  List<OfferDto> GetAllFavourteApplicantOffers(string applicantEmail)
        {
            var offers = mapper.Map<List<OfferDto>>(offerRepository.GetEntities()
               .Include(x => x.Company)
               .ThenInclude(x => x.City)
               .Include(x => x.OfferLanguages)
               .ThenInclude(x => x.Language)
               .Include(x => x.OfferKnowledges)
               .ThenInclude(x => x.Knowledge)
              .Include(x => x.OfferJobCategories)
              .ThenInclude(x => x.JobCategory)
              .Include(x => x.ApplicantOffersFavourites)
              .ThenInclude(x => x.Applicant).Where(x => x.ApplicantOffersFavourites.Any(s => s.ApplicantEmail.Equals(applicantEmail)))).ToList();
            return offers;
        }
        public List<ApplicantOffersFavouriteDto> applicantOffersFavouriteDtos(string applicantEmail)
        {
            var applicantOffersFavouriteDtos = mapper.Map<List<ApplicantOffersFavouriteDto>>(applicantOfferFavouritRepository.GetEntities());
            return applicantOffersFavouriteDtos;
        }
        public async Task<bool> DeleteFavouriteOffer(int id,string applicantEmail)
        {
            if (id == 0) return false;
            var applicantFavoutiteOffer=await applicantOfferFavouritRepository.GetEntities().SingleOrDefaultAsync(x => x.OfferId == id && x.ApplicantEmail.Equals(applicantEmail));
            if(applicantFavoutiteOffer== null) return false;
            try
            {
                applicantOfferFavouritRepository.DeleteEntity(applicantFavoutiteOffer);
                await applicantOfferFavouritRepository.SaveChange();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        public int GetCountFavouritApplicant(string applicantEmail)
        {
            var count=applicantOfferFavouritRepository.GetEntities().Where(x=>x.ApplicantEmail.Equals(applicantEmail)).Count();
            return count;
        }
        #endregion
    }

}
