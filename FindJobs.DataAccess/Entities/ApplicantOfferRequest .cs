using FindJobs.Domain.Enums;
using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantOfferRequest : BaseEntity
    {
        public int Id { get; set; }
        public string ApplicantEmail { get; set; }
        public int OfferId { get; set; }
        public List<OfferDocuments> OfferDocuments { get; set; }
        public OfferStatus Status { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual Offer offer { get; set; }
        
    }

}
