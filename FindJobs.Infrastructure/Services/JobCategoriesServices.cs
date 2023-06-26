using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindJobs.Domain.Services;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace FindJobs.Infrastructure.Services
{
    public class JobCategoriesServices : IJobCategoriService
    {
        private readonly IGenericRepositorySimple<JobCategory> jobCategoriesRepository;
        private readonly IMapper mapper;

        public JobCategoriesServices(IGenericRepositorySimple<JobCategory> jobCategoriesRepository, IMapper mapper)
        {
            this.jobCategoriesRepository = jobCategoriesRepository;
            this.mapper = mapper;
        }

        public void Dispose()
        {
            jobCategoriesRepository?.Dispose();
        }

        public async Task<List<JobCategoryDto>> GetJobCategories()
        {
            var data = await jobCategoriesRepository.GetEntities().ToListAsync();
            return mapper.Map<List<JobCategoryDto>>(data);
        }

        public PaginationDto<JobCategoryDto> GetJobCategoryPagination(int pageNumber = 1, int pageSize = 20, string search = "")
        {
            var model = jobCategoriesRepository.GetEntities().AsNoTracking();
            if (search != null && search != "")
            {
                model = model.Where(x => x.Jobcategory.ToLower().Trim().StartsWith(search.ToLower()));
            }

            var count = model.Count();
            var skip = Math.Min((pageNumber - 1) * pageSize, count - 1);
            var jobCategories = model.Skip(skip).Take(pageSize).ToList();
            var jobCategoryDtos = mapper.Map<List<JobCategoryDto>>(jobCategories);
            var result = new PaginationDto<JobCategoryDto>
            {
                Data = jobCategoryDtos,
                PageCount = (int)Math.Ceiling(((double)count / pageSize)),
                ItemsCount = count
            };
            return result;
        }


    }
}
