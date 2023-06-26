namespace FindJobs.Domain.Dtos.Email
{
    public class SendVerificationCode:EmailBody
    {
        public string Email { get; set; }
        public string VerifyCode { get; set; }

    }
}
