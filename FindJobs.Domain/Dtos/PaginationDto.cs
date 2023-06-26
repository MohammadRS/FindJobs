using DotNek.Common.Models;
using System;
using System.Collections.Generic;

namespace FindJobs.Domain.Dtos
{
    public class PaginationDto<T>
    {
        public int PageCount { get; set; }
        public int ItemsCount { get; set; }
        public  List<T> Data  { get; set; }
    }
}
