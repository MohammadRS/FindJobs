using FindJobs.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class Offer : BaseEntity
    {
        public int Id { get; set; }
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

        #region Relations
        public virtual Company Company { get; set; }
        public virtual List<OfferLanguage> OfferLanguages { get; set; }
        public virtual List<OfferKnowledge> OfferKnowledges { get; set; }
        public virtual List<OfferJobCategory> OfferJobCategories { get; set; }
        public virtual List<ApplicantOfferRequest> ApplicantOfferRequests { get; set; }
        public virtual List<OfferDocuments> OfferDocuments { get; set; }
        public virtual ICollection<ApplicantOffersFavourite> ApplicantOffersFavourites { get; set; }
        #endregion
    }
}

