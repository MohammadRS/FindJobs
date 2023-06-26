namespace FindJobs.Web.Models
{
    public class EmailPreferenceViewModel
    {
        public string NewsLetter { get; set; }
        public string JobNotification { get; set; }
        public string ApplicantEmail { get; set; }
        public string Category { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
