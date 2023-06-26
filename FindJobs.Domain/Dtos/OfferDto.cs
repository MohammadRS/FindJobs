using FindJobs.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class OfferDto:BaseClassDto
    {
        public string CompanyEmail { get; set; }
        public string JobTitle { get; set; }
      
        public string JobDescription { get; set; }
        public string Benifits { get; set; }
        public bool IsFullTime { get; set; }
        public bool IsPartTime { get; set; }
        public bool Agreement { get; set; }
        public bool Freelancer { get; set; }
        public bool IsInternShip { get; set; }
        public bool IsWorkingFromHome { get; set; }
        public bool IsPartialWorkFromHome { get; set; }
        public bool IsWorkAtTheRegularWorkPlace { get; set; }
        public string WorkPlaceAddress { get; set; }
        public bool IsTravelRequirement { get; set; }
        public List<int> RequiredJuniorKnowledges { get; set; }
        public List<int> RequiredSeniorKnowledges { get; set; }
        public List<int> OptionalKnowledges { get; set; }
        public List<int> JobCategories { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? UpperLimit { get; set; }
        public string CurrencyCode { get; set; }
        public PerType PerType { get; set; }
        public bool DoShowApplicantsSalary { get; set; }
        public bool HasPrimaryEducation { get; set; }
        public bool HasHighSchoolStudent { get; set; }
        public bool HasSecondarySchoolWithGraduation { get; set; }
        public bool HasSecondarySchoolWithOutGraduation { get; set; }
        public bool HasPostSecondary { get; set; }
        public bool HasUniversityStudent { get; set; }
        public bool HasHigherEducationLevel1 { get; set; }
        public bool HasHigherEducationLevel2 { get; set; }
        public bool HasHigherEducationLevel3 { get; set; }
        public List<int> LanguageSkillsOptionals { get; set; }
        public List<int> LanguageSkillsRequireds { get; set; }
        public bool HasDriverLicenceA { get; set; }
        public bool HasDriverLicenceB { get; set; }
        public bool HasDriverLicenceC { get; set; }
        public bool HasDriverLicenceD { get; set; }
        public bool HasDriverLicenceE { get; set; }
        public decimal Price { get; set; }
        public decimal Vat { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime ExprationDate { get; set; }
        public string PackageName { get; set; }
       
        public string Link { get; set; }

        public List<OfferJobCategoryDto> OfferJobCategoryDtos { get; set; }
        public  List<OfferLanguageDto> OfferLanguageDtos { get; set; }
        public  List<OfferKnowledgeDto> OfferKnowledgeDtos { get; set; }
        public List<ApplicantOffersFavouriteDto> applicantOffersFavouriteDtos { get; set; }
        public CompanyDto CompanyDto { get; set; }
    }
}
