﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Dtos
{
    public class KnowledgeDto:BaseClassDto
    {
        public string Name { get; set; }
        public int Category { get; set; }
    }
}
