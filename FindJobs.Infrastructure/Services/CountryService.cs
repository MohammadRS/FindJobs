using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepositorySimple<Country> repository;
        private readonly IMapper mapper;
        public CountryService(IGenericRepositorySimple<Country> repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public List<CountryDto> GetCountries()
        {
            var countries = repository.GetEntities().ToList();
            return mapper.Map<List<CountryDto>>(countries);
        }
    }
}
