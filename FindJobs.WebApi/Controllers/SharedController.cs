using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : ControllerBase
    {
        private readonly IJobCategoriService jobCategoriService;

        public SharedController(IJobCategoriService jobCategoriService)
        {
            this.jobCategoriService = jobCategoriService;
        }

        [HttpGet("GetJobCategories")]
        public async Task<IActionResult> GetJobCategories()
        {
            var JobCategoryList = await jobCategoriService.GetJobCategories();
            return new JsonResult(new ResultDto<List<JobCategoryDto>>(JobCategoryList));
        }

        [HttpGet("GetPaginateJobCategory")]
        public IActionResult GetPaginateJobCategory([FromQuery] int pageNumber = 1, int pageSize = 20, string search = "")
        {
            var model = jobCategoriService.GetJobCategoryPagination(pageNumber, pageSize, search);
            return new JsonResult(new ResultDto<PaginationDto<JobCategoryDto>>(model));
        }


    }
}
