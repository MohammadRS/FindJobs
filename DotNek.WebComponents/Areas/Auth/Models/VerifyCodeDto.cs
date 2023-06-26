namespace DotNek.WebComponents.Areas.Auth.Models
{
    public class SendVerifyCodeViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
    public class verifyCodeViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string VerifyCode { get; set; }
       
    }
}
