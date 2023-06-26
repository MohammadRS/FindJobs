using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.App.helper.Constant;
using FindJobs.Domain.Dtos;
using System.Threading.Tasks;

namespace FindJobs.App.Services.Implements
{
    public class LoadData<T> where T : class
    {
      public  int pageNumber= 1;
      public  int pageSize = 40;
      public  int itemCount = 0;
      public  string textSearch = "";
      
      
        public async Task<ResultDto<PaginationDto<T>>> GetData( string url, string baseUrl = "", int pageSize = 40)
        {
            if (baseUrl == null || baseUrl == "") baseUrl = Urls.ApiPath;
                var resultByLastIndex = await API.GetData<ResultDto<PaginationDto<T>>>(baseUrl,
                      url + $"?pageNumber={pageNumber}&pageSize={pageSize}&cityName={textSearch}","");
               
                return resultByLastIndex;
        }
        public async Task<ResultDto<PaginationDto<T>>> GetFilteredOffers(OffersFilter offersFilter, string url, string baseUrl = "", int pageSize = 40,string cityName = null,string jobCategoryList=null)
        {
           
            if (baseUrl == null || baseUrl == "") baseUrl = Urls.ApiPath;
            try
            {
                var data = new OffersFilter
                {
                    pageNumber = pageNumber,
                    pageSize=20,
                    cityName=cityName,
                    jobCategoryFilters=offersFilter.jobCategoryFilters,
                    CompanyOfferFilters=offersFilter.CompanyOfferFilters,
                    JobOfferExpireTimeFilters=offersFilter.JobOfferExpireTimeFilters,
                    LanguageSkillFilters=offersFilter.LanguageSkillFilters,
                    TypeOfEmployeeFilters = offersFilter.TypeOfEmployeeFilters,
                    WorkAreaFilters = offersFilter.WorkAreaFilters

                };
                var resultByLastIndex = await API.PostData<ResultDto<PaginationDto<T>>>(baseUrl,
                                url ,data );
                return resultByLastIndex;
            }
            catch (System.Exception)
            {

                throw;
            }
          

           
        }
    }
}
