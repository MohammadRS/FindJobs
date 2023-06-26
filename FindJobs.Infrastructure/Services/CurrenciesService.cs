using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class CurrenciesService : ICurrenciesService
    {

        private readonly IGenericRepositorySimple<Currency> currencyRepository;
        private readonly IMapper mapper;

        public CurrenciesService(IGenericRepositorySimple<Currency> currencyRepository, IMapper mapper)
        {
            this.currencyRepository = currencyRepository;
            this.mapper = mapper;
        }
        public List<CurrencyDto> GetCurrencies()
        {
            var currencies = currencyRepository.GetEntities().ToList();
            return mapper.Map<List<CurrencyDto>>(currencies);
        }

        public void Dispose()
        {
            currencyRepository?.Dispose();
        }
    }
}
