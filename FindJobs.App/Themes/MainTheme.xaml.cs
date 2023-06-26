using DotNek.AppComponents.Common;
using DotNek.AppComponents.Power;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FFImageLoading.Svg.Forms;
using FindJobs.App.helper;
using FindJobs.App.Pages;
using FindJobs.Domain.Dtos;
using FindJobs.Resources;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Themes
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTheme : ContentPage
    {
        public string CurrentFlag => AppResources.GetImageSvg("Flags.us.svg");
       public static bool isVisible = true;
        CityDto SelectedCity;
     
        
        public MainTheme()
        {
            InitializeComponent();
            ControlTemplate = (ControlTemplate)Resources["MainThemeTemplate"];

            var gradientStart = (GradientStop)GetTemplateChild("gradientStart");
            gradientStart.Color = Color.FromHex(AppSettings.Get("UISettings:ColorTeal"));

            var gradientEnd = (GradientStop)GetTemplateChild("gradientEnd");
            gradientEnd.Color = Color.FromHex(AppSettings.Get("UISettings:ColorAqua"));

            BindingContext = this;
           
            
        }
        protected override  void OnAppearing()
        {
            var headerMain = (StackLayout)GetTemplateChild("HeaderMain");
            if (isVisible == false)
            {
              
                headerMain.SetValue(IsVisibleProperty, false);
            }
            else
            {
                headerMain.SetValue(IsVisibleProperty, true);

            }




            Task.Run(async () =>
            {
                var token = await SecureStorage.GetAsync("AuthToken");
                if (token != null)
                {
                    var result = await API.GetData<ResultDto<int>>(helper.Constant.Urls.ApiPath, "Applicant/GetCountFavouritApplicant", token);
                    if (result != null )
                    {
                        var RecordCount = result.Data;

                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                var suggestImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
                                if (RecordCount > 0)
                                {
                                   

                                    suggestImage.Source = AppResources.GetImageSvg("SharedIcons.heartRed.svg");
                                }
                                else
                                {
                                    suggestImage.Source = AppResources.GetImageSvg("SharedIcons.heart.svg");
                                }

                            });
                        }
                      
                    }
                
               

            });
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
       
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            var mainStack = (StackLayout)GetTemplateChild("MainLayout");
            if (width > height)
            {
                mainStack.Orientation = StackOrientation.Horizontal;
            }
            else
            {
                mainStack.Orientation = StackOrientation.Vertical;
            }

        }
        private async void MenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new SideMenu(), false);
        }
        private async void MenuLanguageClicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await Navigation.PushPopupAsync(new MenuLanguage(), false);
            }
            else
            {
                await DisplayAlert(Res.Messages.Warning, Res.Messages.DontAccessToInternet, Res.Messages.Cancel);
            }
        }

        private async void FindNav_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.Find();
           
        }

        private async void MyJobNav_clicked(object sender, ClickedEventArgs e)
        {
            await Nav.MyJob();
            
        }

        private async void SuggestNav_clicked(object sender, ClickedEventArgs e)
        {
            await Nav.Suggest();
            
        }

        private async void JobAlert_clicked(object sender, ClickedEventArgs e)
        {
            await Nav.JobAlert();
           
        }

        private async void ProfileNav_clicked(object sender, ClickedEventArgs e)
        {
          
            await Nav.Profile();
           
        }
        private async void JobCategory_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var pageInfo = new JobCategorySelectPage();
                pageInfo.dataEventHandler += (popupsender, categoryList) =>
                {
                    var myData = categoryList;
                    var jobcategory = (PowerEntry)GetTemplateChild("JobCategoryList");
                    if (myData == "")
                    {
                        jobcategory.Text = "";
                        return;
                    }
                    jobcategory.Text = categoryList.Remove(categoryList.Length - 1);
                };
                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            else
            {
              await DisplayAlert(Res.Messages.Warning, Res.Messages.DontAccessToInternet,Res.Messages.Cancel);
            }
        }

        private async void CityLocation_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var cityPageInfo = new CityListSelectPage();
                cityPageInfo.cityEventHandler += (popupSender, CityItem) =>
                {
                    var cityLocation = (PowerEntry)GetTemplateChild("CityLocation");
                    SelectedCity = CityItem;
                    cityLocation.Text = SelectedCity.Name;
                    
                };
                await PopupNavigation.Instance.PushAsync(cityPageInfo);
            }
            else
            {
                await DisplayAlert(Res.Messages.Warning, Res.Messages.DontAccessToInternet, Res.Messages.Cancel);
            }
        }

        private async  void SearchOffers_Clicked(object sender, EventArgs e)
        {
            var cityLocation = (PowerEntry)GetTemplateChild("CityLocation");
            var jobcategory = (PowerEntry)GetTemplateChild("JobCategoryList");
            var cityName=cityLocation.Text;
            var jobcategoryList=jobcategory.Text;
            await Application.Current.MainPage.Navigation.PushAsync(new FindPage(cityName,jobcategoryList));
        }
    }
}