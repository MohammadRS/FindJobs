using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface ICurrenciesService : IDisposable
    {
        List<CurrencyDto> GetCurrencies();
    }
}
