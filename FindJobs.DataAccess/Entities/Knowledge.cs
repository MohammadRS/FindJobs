using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class Knowledge:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }

        #region Relations
        public List<OfferKnowledge> OfferKnowledges { get; set; }

        #endregion
    }
}
