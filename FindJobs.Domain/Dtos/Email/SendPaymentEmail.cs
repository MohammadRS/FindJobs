namespace FindJobs.Domain.Dtos.Email
{
    public class SendPaymentEmail : EmailBody
    {
        public string SafeRedirectUrl { get; set; }
        public string Product { get; set; }
        public string Email { get; set; }
        public double Price { get; set; }
        public string PayPalPaymentLink { get; set; }
        public string CrditCardPaymentLink { get; set; }
        public double VAT { get; set; }
        public double PayableAmount { get; set; }
        public int OfferId { get; set; }
        public string CurrencyType { get; set; }

    }
}
