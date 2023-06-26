using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace FindJobs.Infrastructure.Services
{
    public class CultureService : ICultureService
    {
        private readonly IGenericRepositorySimple<Culture> cultureRepositorySimple;
        private readonly IGenericRepositorySimple<Country> countryRepositorySimple;
        private readonly IMapper mapper;

        public CultureService(IGenericRepositorySimple<Culture> cultureRepositorySimple, IGenericRepositorySimple<Country> countryRepositorySimple, IMapper mapper)
        {
            this.cultureRepositorySimple = cultureRepositorySimple;
            this.countryRepositorySimple = countryRepositorySimple;
            this.mapper = mapper;
        }
        public async Task<List<CultureDto>> GetCultures()
        {
            var data = await cultureRepositorySimple.GetEntities()
                .Include(x => x.Countries).Select(x => new CultureDto
                {
                    Language = x.Language,
                    ContinentCode = x.Countries.ContinentCode,
                    CountryCode = x.CountryCode
                }).ToListAsync();
            return mapper.Map<List<CultureDto>>(data);
        }
        public Task<CultureDto> GetCultureData(string culture)
        {
            var ci = CultureInfo.GetCultureInfo(culture);
            var ri = new RegionInfo(culture);
            var currentCountry = countryRepositorySimple.GetEntities().First(x => x.Code == ri.TwoLetterISORegionName);
            var cd = new CultureDto
            {
                CountryCode = ri.TwoLetterISORegionName,
                Language = ci.TwoLetterISOLanguageName,
                ContinentCode = currentCountry.ContinentCode
            };
            return Task.FromResult(cd);
        }

        public Task SetCulture(string culture)
        {
            var newCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            cultureRepositorySimple?.Dispose();
        }
    }
}