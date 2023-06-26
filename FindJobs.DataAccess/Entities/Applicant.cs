using FindJobs.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindJobs.DataAccess.Entities
{
    public class Applicant : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public long? CityId { get; set; }
        public string CityName { get; set; }
        public string Currency { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public bool? IsDrivingLicense { get; set; }
        public bool? IsEuropeanUnion { get; set; }
        public bool? IsSwitzerland { get; set; }
        public bool? IsUnitedStatesofAmerica { get; set; }
        public bool? IsHourlyRate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyAverage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyFrom { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyUntil { get; set; }
        public bool? IsWorkPlace { get; set; }
        public bool? IsWorkFromHome { get; set; }
        public bool? IsPartialworkfromhome { get; set; }
        public bool? IsFullTime { get; set; }
        public bool? IsPartTime { get; set; }
        public bool? IsFreelancer { get; set; }
        public bool? IsInternShip { get; set; }
        public RateType RateType { get; set; }
        public LicenseType? LicenseType { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? AvailableDate { get; set; }
        public ReadyToWork ReadyToWorkStatus { get; set; }
        public string ApplicantImage { get; set; }
        public string ApplicantGoogleImage { get; set; }
        public ProfileVisible ProfileVisible { get; set; }
        public bool AllowSearchEngines { get; set; }
        public bool ShowGender { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowPhone { get; set; }
        public bool ShowCountryOrCity { get; set; }
        public string JobPosition { get; set; }
        public string StateName { get; set; }
        #region Relations
        public virtual List<ApplicantPreference> ApplicantPreferences { get; set; }
        public virtual List<ApplicantWorkExperience> ApplicantWorkExperiences { get; set; }
        public virtual List<ApplicantEducation> ApplicantEducations { get; set; }
        public virtual List<ApplicantKnowledge> ApplicantKnowledges { get; set; }
        public virtual List<ApplicantLanguage> ApplicantLanguages { get; set; }
        public virtual List<ApplicantDocuments> ApplicantDocuments { get; set; }
        public virtual List<ApplicantOfferRequest> ApplicantOfferRequests { get; set; }
        public virtual ICollection<ApplicantOffersFavourite> ApplicantOffersFavourites { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        #endregion

    }
}
