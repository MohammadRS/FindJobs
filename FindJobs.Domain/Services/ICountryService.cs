using FindJobs.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface ICountryService
    {
        List<CountryDto> GetCountries();
    }
}
