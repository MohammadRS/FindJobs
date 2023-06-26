using FindJobs.App.Services.Implements;
using FindJobs.Domain.Dtos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenu : PopupPage
    {
        private ObservableCollection<CityDto> observableCityList;
        private ObservableCollection<JobCategoryDto> observableJobCategory;
        LoadData<JobCategoryDto> loadDataJobCategories = new LoadData<JobCategoryDto>();
        LoadData<CityDto> loadDatacity = new LoadData<CityDto>();
        public SideMenu()
        {
            InitializeComponent();
            checkLoginOrLogout();
        }
        private async void checkLoginOrLogout()
        {
            var token = await SecureStorage.GetAsync("AuthToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                loginButton.SetValue(IsVisibleProperty, true);
                logoutButton.SetValue(IsVisibleProperty, false);
            }
            else
            {
                loginButton.SetValue(IsVisibleProperty, false);
                logoutButton.SetValue(IsVisibleProperty, true);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = informationLoadAsync();


        }
        private async Task informationLoadAsync()
        {
            var cityModel = await loadDatacity.GetData("Culture/GetPaginateCity");
            loadDatacity.pageNumber = 1;
            observableCityList = new ObservableCollection<CityDto>(cityModel.Data.Data.ToList());
            cityCollection.ItemsSource = observableCityList;

            var jobCategoryModel = await loadDataJobCategories.GetData("shared/GetPaginateJobCategory");
            loadDataJobCategories.pageNumber = 1;
            observableJobCategory = new ObservableCollection<JobCategoryDto>(jobCategoryModel.Data.Data.ToList());
            JobCollection.ItemsSource = observableJobCategory;
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);
        }

        private async void logoutButton_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("AuthToken");
            SecureStorage.Remove("LoginType");
            await Navigation.PopPopupAsync();
        }
        private async void JobCollection_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.LastVisibleItemIndex == loadDataJobCategories.pageSize * loadDataJobCategories.pageNumber - 3)
            {

                loadDataJobCategories.pageNumber += 1;
                var jobCategoryModel = await loadDataJobCategories.GetData("shared/GetPaginateJobCategory");
                if (jobCategoryModel != null)
                {

                    foreach (var item in jobCategoryModel.Data.Data)
                    {
                        observableJobCategory.Add(item);
                    }
                    JobCollection.ItemsSource = observableJobCategory;

                }
            }
        }


        private async void cityCollection_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.LastVisibleItemIndex == loadDatacity.pageSize * loadDatacity.pageNumber - 3)
            {

                loadDatacity.pageNumber += 1;
                var list = await loadDatacity.GetData("Culture/GetPaginateCity");
                if (list != null)
                {

                    foreach (var item in list.Data.Data)
                    {
                        observableCityList.Add(item);
                    }
                    cityCollection.ItemsSource = observableCityList;

                }
            }
        }


    }
}