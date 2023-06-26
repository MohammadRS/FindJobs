namespace FindJobs.Domain.Dtos
{
    public class OfferLanguageDto:BaseClassDto
    {
        public int OfferId { get; set; }
        public int LanguageId { get; set; }
        public int LanguageLevel { get; set; }

        public LanguageDto LanguageDto { get; set; }
    }
}
