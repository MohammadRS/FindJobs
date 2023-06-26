namespace FindJobs.DataAccess.Entities
{
    public class OfferDocuments : BaseEntity
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual ApplicantDocuments Document { get; set; }

    }

}
