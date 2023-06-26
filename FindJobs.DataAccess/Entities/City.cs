using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class City:BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AsciiName { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string CountryCode { get; set; }
        public string Iso3 { get; set; }
        public string StateName { get; set; }
        public string Capital { get; set; }
        public string Population { get; set; }

        #region Relations
      
        public List<Applicant> Applicants { get; set; }
        public List<Company> Companies { get; set; }
        #endregion
    }
}