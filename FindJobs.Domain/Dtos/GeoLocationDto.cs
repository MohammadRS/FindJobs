namespace FindJobs.Domain.Dtos
{
    public class GeoLocationDto
    {
        public long FromIp { get; set; }
        public long ToIp { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Longitude { get; set; }
        public string Zipcode { get; set; }
        public string Timezone { get; set; }
        public string PhonePrefix { get; set; }
    }
}