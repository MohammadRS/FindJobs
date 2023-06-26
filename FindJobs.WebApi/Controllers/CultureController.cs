using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNek.Common.Models;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using FindJobs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        private readonly ICultureService cultureService;
        private readonly IGeoLocationService geoLocationService;
        private readonly ICitiesService citiesService;
        private readonly ICountryService countryService;
        private readonly ICurrenciesService currenciesService;

        public CultureController(ICultureService cultureService, IGeoLocationService geoLocationService, ICitiesService citiesService,ICountryService countryService, ICurrenciesService currenciesService)
        {
            this.cultureService = cultureService;
            this.geoLocationService = geoLocationService;
            this.citiesService = citiesService;
            this.countryService = countryService;
            this.currenciesService = currenciesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCultures()
        {
            var cultureList = await cultureService.GetCultures();
            return new JsonResult(new ResultDto<List<CultureDto>>(cultureList));
        }

        [HttpPost("GetCultureByIp")]
        public async Task<IActionResult> GetCultureByIp([FromBody] CultureResponseDto data)
        {
            var cultureDto = await geoLocationService.GetCultureByIp(data.Ip, data.BrowserLanguage);
            return new JsonResult(new ResultDto<CultureDto>(cultureDto));
        }
        [HttpGet("GetCities")]
        [ResponseCache(Duration = 864000)]
        public IActionResult GetCities([FromQuery] string countryCode = null)
        {
            if (countryCode == null)
            {
               return new JsonResult(new ResultDto<List<CityDto>>(citiesService.GetCities()));
            }
            else
            {
                return new JsonResult(new ResultDto<List<CityDto>>(citiesService.GetCities(countryCode)));
                   
                
            }
        }
        [HttpGet("GetPaginateCity")]
        public IActionResult GetPaginateCity([FromQuery] int pageNumber=1,int pageSize=20,string cityName="")
        {
           var model= citiesService.GetCitiesByPagination(pageNumber, pageSize, cityName);
            return new JsonResult(new ResultDto<PaginationDto<CityDto>>(model));
        }

        [HttpGet("GetCurrencies")]
        [ResponseCache(Duration = 864000)]
        public IActionResult GetCurrencies() => new JsonResult(new ResultDto<List<CurrencyDto>>(currenciesService.GetCurrencies()));

        [HttpGet("GetCountries")]
        [ResponseCache(Duration = 864000)]
        public IActionResult GetCountries() => new JsonResult(new ResultDto<List<CountryDto>>(countryService.GetCountries()));

        [HttpGet("GetCitiesByCountryName/{countryCode}")]
        public IActionResult GetCitiesByCountryName(string countryCode)
        {
            var cities = citiesService.GetCitiesByCountryCode(countryCode).OrderBy(x => x.Name).ToList();
            return new JsonResult(new ResultDto<List<CityDto>>(cities));
        }
    } 
}
