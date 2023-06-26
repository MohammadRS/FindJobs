using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindJobs.Domain.Dtos;

namespace FindJobs.Domain.Services
{

    public interface IJobCategoriService : IDisposable
    {
        Task<List<JobCategoryDto>> GetJobCategories();
        PaginationDto<JobCategoryDto> GetJobCategoryPagination(int pageNumber = 1, int pageSize = 20, string search = "");
    }
}
