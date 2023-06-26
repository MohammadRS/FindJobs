using FindJobs.Domain.Enumes;
using System;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class ApplicantDocumentsDto:BaseClassDto
    {
        private Dictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
            { ".jpg", "image/jpeg"},
            { ".jpeg", "image/jpeg"},
            { ".png", "image/png"},
            { ".pdf", "application/pdf"}
        };
        public string ApplicantEmail { get; set; }
        public string DocumentFile { get; set; }
      
        public string ExtensionFile { get; set; }
        public string DocumentFileBase64
        {
            get
            {
                return "data:" + MimeTypes[ExtensionFile] + ";base64," + DocumentFile;
            }
        }
        public UploadDocumentType Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

