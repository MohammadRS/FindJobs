namespace FindJobs.DataAccess.Entities
{
    public class OfferLanguage:BaseEntity
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public int LanguageId { get; set; }
        public int LanguageLevel { get; set; }
       
        #region Relations
        public Offer Offer { get; set; }
        public Language Language { get; set; }
        #endregion

    }
}
