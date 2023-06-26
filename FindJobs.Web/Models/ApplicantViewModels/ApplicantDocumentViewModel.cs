using FindJobs.Domain.Enumes;
using Microsoft.AspNetCore.Http;

namespace FindJobs.Web.Models.ApplicantViewModels
{
    public class ApplicantDocumentViewModel
    {
        public IFormFile DocumentFile { get; set; }
        public string ExtensionFile { get; set; }
        public UploadDocumentType Type { get; set; }
    }
}
