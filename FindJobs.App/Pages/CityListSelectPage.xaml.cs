using FindJobs.App.Services.Implements;
using FindJobs.Domain.Dtos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityListSelectPage : PopupPage
    {
        ObservableCollection<CityDto> observableCityList;
       public EventHandler<CityDto> cityEventHandler;
        LoadData<CityDto> loadDataInstance = new LoadData<CityDto>();
        

        public CityListSelectPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await FillCities();

        }

        private async Task FillCities()
        {
            var model = await loadDataInstance.GetData("Culture/GetPaginateCity");
            loadDataInstance.pageNumber = 1;
            observableCityList = new ObservableCollection<CityDto>(model.Data.Data.ToList());
            CollectionCity.ItemsSource = observableCityList;
        }

        private  void serchCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (e.NewTextValue.Length > 1)
            {
                Task.Run(async () =>
                {
                    loadDataInstance.textSearch = e.NewTextValue.ToString();
                    loadDataInstance.pageNumber = 1;
                    await Task.Delay(1000);
                    var list = await loadDataInstance.GetData("Culture/GetPaginateCity");
                    observableCityList.Clear();
                    foreach (var item in list.Data.Data)
                    {
                        observableCityList.Add(item);
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        CollectionCity.ItemsSource = observableCityList;
                        stackIndicator.SetValue(IsVisibleProperty, false);
                    });
                   
                });
               
            }
        }


        private async void CollectionCity_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.LastVisibleItemIndex == loadDataInstance.pageSize * loadDataInstance.pageNumber - 3)
            {
                stackIndicator.SetValue(IsVisibleProperty, true);
                loadDataInstance.pageNumber += 1;
                var list = await loadDataInstance.GetData("Culture/GetPaginateCity");
                if (list != null)
                {
                 
                    foreach (var item in list.Data.Data)
                    {
                        observableCityList.Add(item);
                    }
                    CollectionCity.ItemsSource = observableCityList;
                    stackIndicator.SetValue(IsVisibleProperty, false);
                }
            }
        }
    

       
        private async void CollectionCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection[0] as CityDto;
            cityEventHandler?.Invoke(this, item);
            await Navigation.PopPopupAsync();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

       
    }
}