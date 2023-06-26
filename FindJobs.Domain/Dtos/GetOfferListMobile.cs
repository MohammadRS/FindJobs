using System;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class GetOfferListMobile:BaseClassDto
    {
        public string JobTitle { get; set; }
       public string Logo { get; set; }
        public string CompanyEmail { get; set; }
        public List<string> JobCategoryNameList { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public DateTime DateOfOffer { get; set; }
        public string FavouriteIcon { get; set; }
    }
}
