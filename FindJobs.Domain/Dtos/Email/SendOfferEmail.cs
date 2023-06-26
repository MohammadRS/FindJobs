using System.Collections.Generic;

namespace FindJobs.Domain.Dtos.Email
{
    public class SendOfferEmail:EmailBody
    {
        public List<OfferDto> Model { get; set; }
        public  string UnsubscribeLink { get; set; }
    }
}
