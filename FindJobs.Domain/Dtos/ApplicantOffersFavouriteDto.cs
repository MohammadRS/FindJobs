using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantOffersFavouriteDto:BaseClassDto
    {
        public string ApplicantEmail { get; set; }
        public int OfferId { get; set; }
    }

    public enum ApplicantOffersFavouriteResult
    {
        Success=0,
        Badrequest=1,
        AddedBefore=2

    }
}
