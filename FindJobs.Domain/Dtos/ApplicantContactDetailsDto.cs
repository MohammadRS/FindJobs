namespace FindJobs.Domain.Dtos
{
    public class ApplicantContactDetailsDto
    {
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public long? CityId { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityMainName { get; set; }
    }
}
