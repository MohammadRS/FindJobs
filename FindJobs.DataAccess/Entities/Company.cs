using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindJobs.DataAccess.Entities
{
    public class Company : BaseEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public bool IsTop { get; set; }
        public int CompanyRegistrationId { get; set; }
        public decimal VatNumber { get; set; }
        public decimal TaxNumber { get; set; }
        public string WebSite { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public long? CityId { get; set; }

        #region ContactPerson
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NumberOfEmployee { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        #endregion

        public string AboutCompany { get; set; }

        #region relations
        public Country Country { get; set; }
        public City City { get; set; }
        #endregion

    }
}
