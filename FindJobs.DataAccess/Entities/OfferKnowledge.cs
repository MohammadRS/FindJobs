using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Entities
{
    public class OfferKnowledge:BaseEntity
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public int KnowledgeId { get; set; }
        public int KnowledgeLevel { get; set; }

        #region Relations
        public Offer Offer { get; set; }
        public Knowledge Knowledge { get; set; }
        #endregion
    }
}
