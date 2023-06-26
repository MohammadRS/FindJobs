using System.Globalization;

namespace FindJobs.Domain.Dtos
{
    public class CultureDto
    {
        public string CountryCode { get; set; } = "US";
        public string Language { get; set; } = "en";
        public string CultureCode => $"{Language}-{CountryCode}";

        public string CountryNativeName
        {
            get
            {
                var nativeName = CultureInfo.GetCultureInfo(CultureCode).NativeName.Split('(', ')', ',');
                return nativeName.Length <= 1 ? null : nativeName[1].Trim();
            }
        }
        public string CountryEnglishName => CultureInfo.GetCultureInfo(CultureCode).EnglishName.Split('(', ')', ',')[1].Trim();
        public string LanguageNativeName => CultureInfo.GetCultureInfo(CultureCode).NativeName.Split('(', ')', ',')[0].Trim();
        public string ContinentCode { get; set; } 
    }
    public class SeedDataCultureDto
    {
        public string CountryCode { get; set; }
        public string Language { get; set; }
    }
    public class CultureResponseDto
    {
        public string Ip { get; set; }
        public string[] BrowserLanguage { get; set; }
    }
}