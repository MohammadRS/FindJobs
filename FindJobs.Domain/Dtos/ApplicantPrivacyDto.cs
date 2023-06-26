using FindJobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantPrivacyDto
    {
        public string Email { get; set; }
        public ProfileVisible ProfileVisible { get; set; }
        public bool AllowSearchEngines { get; set; }
        public bool ShowGender { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowCountryOrCity { get; set; }
        public bool ShowPhone { get; set; }

    }
}
