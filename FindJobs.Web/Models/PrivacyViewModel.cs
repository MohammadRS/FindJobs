using FindJobs.Domain.Enums;

namespace FindJobs.Web.Models
{
    public class PrivacyViewModel
    {
        public string Email { get; set; }
        public ProfileVisible ProfileVisible { get; set; }
        public bool AllowSearchEngines { get; set; }
        public bool ShowNationality { get; set; }
        public bool ShowGender { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowCountry { get; set; }
    }
}
