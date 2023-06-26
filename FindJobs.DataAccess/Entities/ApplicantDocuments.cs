using FindJobs.Domain.Enumes;
using System.Collections.Generic;

namespace FindJobs.DataAccess.Entities
{
    public class ApplicantDocuments : BaseEntity
    {
        public int Id { get; set; }
        public string DocumentFile { get; set; }
        public string ExtensionFile { get; set; }
        public UploadDocumentType Type { get; set; }
        public string ApplicantEmail { get; set; }


        #region Relations
        public virtual Applicant Applicant { get; set; }
        public virtual List<OfferDocuments> OfferDocuments { get; set; }
        #endregion

    }
}
