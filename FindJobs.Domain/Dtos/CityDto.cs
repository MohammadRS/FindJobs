namespace FindJobs.Domain.Dtos
{
    public class CityDto
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
    }
}