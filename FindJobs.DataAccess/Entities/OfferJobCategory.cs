namespace FindJobs.DataAccess.Entities
{
    public class OfferJobCategory:BaseEntity
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public int  OfferId { get; set; }

        #region Relations
        public JobCategory JobCategory { get; set; }
        public Offer Offer { get; set; }
        #endregion
    }
}
