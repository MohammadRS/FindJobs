using FindJobs.Resources;

namespace FindJobs.Domain.Dtos
{
    public class CountryDto
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
        public string Flag => AppResources.GetImageSvg($"Flags.{Code.ToLower()}.svg");

    }
}