using FindJobs.Domain.Enums;
using System;

namespace FindJobs.DataAccess.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string Authority { get; set; }
        public float Amount { get; set; }
        public GetwayTypes GetwayType { get; set; }
        public string Currency { get; set; }
        public bool Done { get; set; }
        public string TransactionId { get; set; }
        public bool IsRefunded { get; set; }
        public void UpdatePayment( string authority, float amount, GetwayTypes getwayType, string currency, bool done, string transeActionId, bool isRefunded)
        {
            Authority = authority;
            Amount = amount;
            this.GetwayType = getwayType;
            Currency = currency;
            Done = done;
            TransactionId = transeActionId;
            IsRefunded = isRefunded;
            LastUpdateDate = DateTime.Now;
        }



    }
}
