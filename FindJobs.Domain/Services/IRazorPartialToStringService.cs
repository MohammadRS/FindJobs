using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IRazorPartialToStringService
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
