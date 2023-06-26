using System.Collections.Generic;
using System.Threading.Tasks;
using FindJobs.Domain.Dtos;

namespace FindJobs.Domain.Services
{
    public interface IGeoLocationService
    {
        Task<CultureDto> GetCultureByIp(string ip, string[] browserLanguage);
    }
}