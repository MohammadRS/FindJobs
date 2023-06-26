using FindJobs.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.ViewModels
{
    public class JobOfferViewModel
    { 
        public CompanyDto CompanyDto { get; set; }
        public List<KnowledgeDto> KnowledgeDtos { get; set; }
        public List<CurrencyDto> CurrencyDtos { get; set; }
        public List<LanguageDto> LanguageDtos { get; set; }
        public List<PackageDto> PackageDtos { get; set; }
        public List<JobCategoryDto> JobCategories { get; set; }
        public OfferDto OfferDto { get; set; }

    }
}
