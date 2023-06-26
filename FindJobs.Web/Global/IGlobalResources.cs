using System.Collections.Generic;
using System.Threading.Tasks;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;

namespace FindJobs.Web.Global
{
    public interface IGlobalResources
    {
        Dictionary<string, string> GetInlineStyles();
    }
}
