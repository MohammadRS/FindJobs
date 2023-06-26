using System.Collections.Generic;
using System.Threading.Tasks;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;

namespace FindJobs.Domain.Global
{
    public class Global
    {
        public static List<string> continentGroups = new List<string>() { "NA", "EU", "SA", "AS,OC,AF" };
        private static List<CultureDto> CultureList { get; set; }
        private static List<CityDto> CityList { get; set; }
        private static List<CountryDto> CountryList { get; set; }
        private static List<CurrencyDto> CurrencyList { get; set; }
        private static List<JobCategoryDto> JobCategoryList { get; set; }

        private static PaginationDto<CityDto> paginationCityDto { get; set; }
      

        public static async Task<List<CultureDto>> GetCultureList(string baseApiUrl)
        {
            if (CultureList != null) return CultureList;
            var result = await API.GetData<ResultDto<List<CultureDto>>>(baseApiUrl,
                "Culture", "");
            CultureList = result.Data ?? new List<CultureDto>();
            return CultureList;
        }
        public static async Task<List<CityDto>> GetCityList(string baseApiUrl,string countryCode=null)
        {
            if (CityList != null) return CityList;
            if(countryCode== null)
            {
                var result = (await API.GetData<ResultDto<List<CityDto>>>
               (baseApiUrl, "Culture/GetCities")).Data;
                CityList = result ?? new List<CityDto>();
                return CityList;
            }
            else
            {
                var result = (await API.GetData<ResultDto<List<CityDto>>>
              (baseApiUrl, "Culture/GetCities?countryCode="+countryCode)).Data;
                CityList = result ?? new List<CityDto>();
                return CityList;
            }
        }

        public static async Task<List<CountryDto>> GetCountryList(string baseApiUrl)
        {
            if (CountryList != null) return CountryList;
            var result = (await API.GetData<ResultDto<List<CountryDto>>>
                (baseApiUrl, "Culture/GetCountries")).Data;
            CountryList = result ?? new List<CountryDto>();
            return CountryList;
        }

        public static async Task<List<CurrencyDto>> GetCurrencyList(string baseApiUrl)
        {
            if (CurrencyList != null) return CurrencyList;
            var result = (await API.GetData<ResultDto<List<CurrencyDto>>>
                (baseApiUrl, "Culture/GetCurrencies")).Data;
            CurrencyList = result ?? new List<CurrencyDto>();
            return CurrencyList;
        }
        public static async Task<List<JobCategoryDto>> GetJobCategories(string baseApiUrl)
        {
            if(JobCategoryList != null) return JobCategoryList;
            var result = (await API.GetData<ResultDto<List<JobCategoryDto>>>
                (baseApiUrl, "Shared/GetJobCategories")).Data;
               JobCategoryList = result ?? new List<JobCategoryDto>();
            return JobCategoryList;
               
        }

       
    }
}