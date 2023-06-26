using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class OfferKnowledgeDto:BaseClassDto
    {
        public int OfferId { get; set; }
        public int KnowledgeId { get; set; }
        public int KnowledgeLevel { get; set; }

        public KnowledgeDto KnowledgeDto { get; set; }
    }
}
