namespace FindJobs.Domain.Dtos
{
    public class ApplicantPreferenceDto:BaseClassDto
    {
        public string ApplicantEmail { get; set; }
        public string Category { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
