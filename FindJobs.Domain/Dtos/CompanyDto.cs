using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace FindJobs.Domain.Dtos
{
    public class CompanyDto:BaseClassDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsTop { get; set; }
        public string Logo { get; set; }
        public int? CompanyRegistrationId { get; set; }
        public decimal? VatNumber { get; set; }
        public decimal? TaxNumber { get; set; }
        public string WebSite { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
       

        #region ContactPerson
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NumberOfEmployee { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        #endregion

        public string AboutCompany { get; set; }
        public IFormFile ImageLogo { get; set; }
        public string CountryCode { get; set; }
        public long? CityId { get; set; }
        public CityDto CityDto { get; set; }
        public List<CountryDto> CountryDtoList { get; set; }
    }
    public enum SendVerificationCodeCompanyResult
    {
        Success,
    }
}
