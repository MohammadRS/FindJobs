namespace FindJobs.DataAccess.Entities
{
    public class GeoLocation
    {
        public int Id { get; set; }
        public long FromIp{ get; set; }
        public long ToIp { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Zipcode { get; set; }
        public string Timezone { get; set; }
    }
}
