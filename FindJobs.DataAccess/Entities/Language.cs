using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Entities
{
    public class Language:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Importance { get; set; }

        #region Relations
        public List<OfferLanguage> OfferLanguages { get; set; }
        #endregion
    }
}
