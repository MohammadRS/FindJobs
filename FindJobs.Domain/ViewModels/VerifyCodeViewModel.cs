namespace FindJobs.Domain.ViewModels
{
    public class VerifyCodeViewModel
    {
        public string Email { get; set; }
        public string Captcha { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
    }
}
