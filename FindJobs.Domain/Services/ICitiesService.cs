using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface ICitiesService : IDisposable
    {
        List<CityDto> GetCities();
        List<CityDto> GetCities(string countryCode);
        List<CityDto> GetCitiesByCountryCode(string countryCode);
        PaginationDto<CityDto> GetCitiesByPagination(int pageNumber = 1, int pageSize = 20,string cityName="");
    }
}
