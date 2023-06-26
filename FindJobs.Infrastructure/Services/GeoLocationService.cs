using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;

namespace FindJobs.Infrastructure.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IGenericRepositorySimple<GeoLocation> geoLocationRepositorySimple;
        private readonly IGenericRepositorySimple<Culture> cultureRepositorySimple;
        private readonly IMapper mapper;

        public GeoLocationService(IGenericRepositorySimple<GeoLocation> geoLocationRepositorySimple, IGenericRepositorySimple<Culture> cultureRepositorySimple,
            IMapper mapper)
        {
            this.geoLocationRepositorySimple = geoLocationRepositorySimple;
            this.cultureRepositorySimple = cultureRepositorySimple;
            this.mapper = mapper;
        }
        public Task<CultureDto> GetCultureByIp(string ip, string[] browserLanguage)
        {
            try
            {
                var ipLocation = IpToNumber(ip);
                var countryCode = (geoLocationRepositorySimple.GetEntities().FirstOrDefault(x => x.FromIp <= ipLocation && x.ToIp >= ipLocation)?.CountryCode) ?? new CultureDto().CountryCode;
                var cultureDto = CalculateCultureByCountry(countryCode, browserLanguage);
                return Task.FromResult(cultureDto);
            }
            catch (Exception)
            {

                return Task.FromResult(new CultureDto());
            }
            
            
        }

        private CultureDto CalculateCultureByCountry(string countryCode, string[] browserLanguage)
        {
            //USER CULTURE
            try
            {
                if (countryCode.Length == 2)
                {
                    if (browserLanguage is { Length: >= 1 } && browserLanguage[0].Length >= 2)
                    {
                        var cultureList = cultureRepositorySimple.GetEntities().ToList();
                        string userLanguage = browserLanguage[0][..2];
                        var userCulture =
                            cultureList.Where(x => x.Language == userLanguage && x.CountryCode == countryCode).ToList();
                        if (userCulture.Any())
                        {
                            //culture choice 1
                            return mapper.Map<CultureDto>(userCulture.First());
                        }
                        else
                        {
                            //culture choice 2
                            var firstLanguageOfCountryCode = cultureList.Where(x => x.CountryCode == countryCode).ToList();
                            if (firstLanguageOfCountryCode.Count > 0)
                                return mapper.Map<CultureDto>(firstLanguageOfCountryCode.First());
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return new CultureDto()
            {
                Language = "en",
                CountryCode = "US"
            };
        }

        private static long IpToNumber(string ip)
        {
            long result = 0;
            if (!ip.Contains(".")) return result;
            var ip1 = int.Parse(ip.Split('.')[0]);
            var ip2 = int.Parse(ip.Split('.')[1]);
            var ip3 = int.Parse(ip.Split('.')[2]);
            var ip4 = int.Parse(ip.Split('.')[3]);
            result = Convert.ToInt64((ip1 * Math.Pow(256, 3)) + (ip2 * Math.Pow(256, 2)) + (ip3 * 256) + ip4);
            return result;
        }
    }
}