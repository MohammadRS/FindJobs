using AutoMapper;
using DotNek.Common.Models;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class CitiesService : ICitiesService
    {

        private readonly IGenericRepositorySimple<City> cityRepository;
        private readonly IMapper mapper;

        public CitiesService(IGenericRepositorySimple<City> cityRepository,IMapper mapper)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
        }
        public List<CityDto> GetCities()
        {
            var cities = cityRepository.GetEntities().ToList();
            return mapper.Map<List<CityDto>>(cities);
        }
        public PaginationDto<CityDto> GetCitiesByPagination(int pageNumber = 1, int pageSize = 20,string cityName="")
        {
            var model = cityRepository.GetEntities().AsNoTracking();
            if(cityName != null && cityName != "")
            {
                model=model.Where(x=>x.Name.ToLower().Trim().StartsWith(cityName.ToLower()));
            }

            if (model.Count() > 0)
            {
                var count = model.Count();
                var skip = Math.Min((pageNumber - 1) * pageSize, count - 1);
                var cities = model.Skip(skip).Take(pageSize).ToList();
                var citiesDto = mapper.Map<List<CityDto>>(cities);
                var result = new PaginationDto<CityDto>
                {
                    Data = citiesDto,
                    PageCount = (int)Math.Ceiling(((double)count / pageSize)),
                    ItemsCount = count
                };
                return result;
            }
            return new PaginationDto<CityDto>();
           
        }
        public List<CityDto> GetCities(string countryCode)
        {
            var cities = cityRepository.GetEntities()
                .Where(x => x.CountryCode.ToLower().Trim().Equals(countryCode.ToLower().Trim()));
            return mapper.Map<List<CityDto>>(cities);
        }

        public List<CityDto> GetCitiesByCountryCode(string countryCode)
        {
            var cities = cityRepository.GetEntities().Where(a => a.CountryCode.Equals(countryCode)).ToList();
            return mapper.Map<List<CityDto>>(cities);
        }
        public void Dispose()
        {
            cityRepository?.Dispose();
        }
    }
}
