using System.Collections.ObjectModel;

namespace FindJobs.Domain.Dtos
{
    public class GroupingContinentsDto : ObservableCollection<CountryDto>
    {
        public string ContinentName { get; set; }
    }
}