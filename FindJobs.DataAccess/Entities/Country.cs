using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class Country:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySign { get; set; }
        public int Importance { get; set; }
        public string ContinentCode { get; set; }
        public string PhonePrefix { get; set; }
        public bool IsEuropeanUnion { get; set; }
        public string Capital { get; set; }

        #region Relations
        public List<City> Cities { get; set; }
        public List<Culture> Cultures { get; set; }
        public List<Applicant> Applicants { get; set; }
        public List<Company> Companies { get; set; }
        #endregion
    }
}