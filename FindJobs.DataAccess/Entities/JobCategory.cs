using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public  class JobCategory
    {
        public int Id { get; set; }
        public string Jobcategory { get; set; }


        #region Relations
        public List<OfferJobCategory> OfferJobCategories { get; set; }
        #endregion
    }
}
