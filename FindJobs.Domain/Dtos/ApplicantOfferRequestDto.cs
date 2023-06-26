using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantOfferRequestDto
    {
        public int Id { get; set; }
        public string ApplicantEmail { get; set; }
        public int OfferId { get; set; }
        public List<OfferDocumentsDto> OfferDocumentsDto { get; set; }
        public int Status { get; set; }
    }
}
