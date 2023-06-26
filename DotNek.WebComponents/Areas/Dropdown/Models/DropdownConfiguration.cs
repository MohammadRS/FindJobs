using System.Collections.Generic;

namespace DotNek.WebComponents.Areas.Dropdown.Models
{
    public class DropdownConfiguration
    {
        public string ComponentId { get; set; }
        public string SelectStatus { get; set; }
        public string Title { get; set; }
        public List<string> Items { get; set; }
        public string Culture { get; set; }

        public string SelectedItemColor { get; set; }

    }
}