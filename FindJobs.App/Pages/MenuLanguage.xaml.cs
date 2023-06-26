using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotNek.AppComponents.Common;
using FindJobs.App.helper.Constant;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuLanguage : PopupPage
    {
        public ObservableCollection<GroupingContinentsDto> LanguageItem { get; set; }

        public MenuLanguage()
        {
            InitializeComponent();
            LanguageItem = new ObservableCollection<GroupingContinentsDto>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetContinentsInformation();
        }
        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        public async Task GetContinentsInformation()
        {
            var getCultures = await Global.GetCultureList(Urls.ApiPath);
            if (getCultures == null) return;
            var continentGroups = Global.continentGroups;

            foreach (var continent in continentGroups)
            {
                var newGroup = new GroupingContinentsDto() { ContinentName = Res.Layout.ResourceManager.GetString(continent) };
                foreach (var item in getCultures.Where(item => continent.Split(',').Contains(item.ContinentCode)))
                {
                    newGroup.Add(new CountryDto() { Name = item.CountryNativeName + " - " + item.LanguageNativeName, Code = item.CountryCode });
                }
                LanguageItem.Add(newGroup);
            }
        }
    }
}