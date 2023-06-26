namespace FindJobs.DataAccess.Entities
{
    public class ApplicantOffersFavourite:BaseEntity
    {
        public int Id { get; set; }
        public string ApplicantEmail { get; set; }
        public int OfferId { get; set; }

        #region Relations
        public Offer Offer { get; set; }
        public Applicant Applicant { get; set; }
        #endregion
    }

}
