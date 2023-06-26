namespace FindJobs.DataAccess.Entities
{
    public class ApplicantPreference
    {
        public int Id { get; set; }
        public string ApplicantEmail { get; set; }
        public string Category { get; set; }
        public bool IsSubscribed { get; set; }

        public Applicant Applicant { get; set; }

    }
}