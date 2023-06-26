using System.Collections.Generic;
using System.Threading.Tasks;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindJobs.Web.Controllers
{
    public class CultureController : BaseController
    {
        public CultureController(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<ResultDto<List<CityDto>>> GetCitiesByCountryCode([FromBody] string countryCode)
        {
            var model = await API.GetData<ResultDto<List<CityDto>>>(configuration["GlobalSettings:ApiUrl"],
                $"Culture/GetCitiesByCountryName/{countryCode}"
                , Request.Cookies["AuthToken"]);
            return model;
        }
    }
}
