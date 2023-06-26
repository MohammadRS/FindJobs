using System;
using System.Collections.Generic;
using System.Text;

namespace FindJobs.App.ViewModels
{
    public class jobCategoryViewModel
    {
        public int Id { get; set; }
        public string JobCategory { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
