using FindJobs.Domain.Enums;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantAddtionalSectionDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsDrivingLicense { get; set; }
        public LicenseType? LicenseType { get; set; }
        public bool IsEuropeanUnion { get; set; }
        public bool IsSwitzerland { get; set; }
        public RateType RateType { get; set; }
        public bool IsUnitedStatesOfAmerica { get; set; }
        public bool IsHourlyRate { get; set; }
        public decimal HourlyAverage { get; set; }
        public decimal HourlyFrom { get; set; }
        public decimal HourlyUntil { get; set; }
        public bool IsWorkPlace { get; set; }
        public bool IsWorkFromHome { get; set; }
        public bool IsPartialWorkFromHome { get; set; }
        public bool IsFullTime { get; set; }
        public bool IsPartTime { get; set; }
        public bool IsFreelancer { get; set; }
        public bool IsInternShip { get; set; }
        public string Currency { get; set; }
    }
}
