using System.Security.Permissions;

namespace FindJobs.Domain.Dtos
{
    public class OfferJobCategoryDto : BaseClassDto
    {
        public int OfferId { get; set; }
        public int JobCategoryId { get; set; }

        public JobCategoryDto JobCategoryDto { get; set; }
    }
}
