using FindJobs.Domain.Dtos;
using System.Collections.Generic;

namespace FindJobs.Domain.ViewModels
{
    public class OfferViewModel
    {
        public OfferDto  offerDto{ get; set; }
        public List<ApplicantDocumentsDto> applicantDocumentsDtos { get; set; }
    }
}



