using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
   

    public class JobCategoryFilter
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Count { get; set; }
        public bool IsChecked { get; set; }=false;
    }
    public class TypeOfEmployeeFilter
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; } = false;
    }
    public class CompanyOfferFilter
    {
        public string ComapnyName { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; } = false;
    }
    public class LanguageSkillFilter
    {
        public int LanguageSkillId { get; set; }
        public string LanguageName { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; } = false;
    }
     public class JobOfferExpireTimeFilter {
        public string JobOfferTimeName { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; } = false;
    }
    public class WorkAreaFilter
    {
        public string WorkAreaName { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; } = false;
    }

    public class OffersFilter
    {
        public OffersFilter()
        {
            jobCategoryFilters = new List<JobCategoryFilter>();
            TypeOfEmployeeFilters = new List<TypeOfEmployeeFilter>();
            CompanyOfferFilters=new List<CompanyOfferFilter>();
            LanguageSkillFilters=new List<LanguageSkillFilter>();
            JobOfferExpireTimeFilters=new List<JobOfferExpireTimeFilter>();
            WorkAreaFilters=new List<WorkAreaFilter>();


        }
        public List<JobCategoryFilter> jobCategoryFilters { get; set; }
        public List<TypeOfEmployeeFilter> TypeOfEmployeeFilters { get; set; }
        public List<CompanyOfferFilter> CompanyOfferFilters { get; set; }
        public List<LanguageSkillFilter> LanguageSkillFilters { get; set; }
        public List<JobOfferExpireTimeFilter> JobOfferExpireTimeFilters { get; set; }
        public List<WorkAreaFilter> WorkAreaFilters { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string cityName { get; set; }
    }
}

