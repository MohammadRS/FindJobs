using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.MapperProfiles
{
    public class KnowledgeProfile:Profile
    {
        public KnowledgeProfile()
        {
            CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        }
    }
}
