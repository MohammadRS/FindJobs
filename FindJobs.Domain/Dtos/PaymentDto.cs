using FindJobs.Domain.Enums;
using System;

namespace FindJobs.Domain.Dtos
{
    public class PaymentDto
    {
        public int OfferId { get; set; }
        public string Authority { get; set; }
        public float Amount { get; set; }
        public GetwayTypes GetwayType { get; set; }
        public string Currency { get; set; }
        public bool Done { get; set; }
        public string TransactionId { get; set; }
        public bool IsRefunded { get; set; }
    }
}
