using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FindJobs.Domain.Dtos;

namespace FindJobs.Domain.Services
{
    public interface ICultureService: IDisposable
    {
        Task<List<CultureDto>> GetCultures();
        Task<CultureDto> GetCultureData(string culture);
        Task SetCulture(string culture);
    }
}