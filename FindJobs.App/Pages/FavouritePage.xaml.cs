using Android.Text;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FFImageLoading.Svg.Forms;
using FindJobs.App.Pages.Offers;
using FindJobs.App.Themes;
using FindJobs.App.ViewModels;
using FindJobs.Domain.Dtos;
using FindJobs.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritePage : MainTheme
    {

        ObservableCollection<OfferViewModel> offers = new ObservableCollection<OfferViewModel>();
        bool favoutireHasRecord = false;

        public FavouritePage()
        {
            InitializeComponent();
           
            intialData();
        }
        protected override void OnAppearing()
        {
            MainTheme.isVisible = false;

            base.OnAppearing();
            if (favoutireHasRecord)
            {
                var ProfileImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
                ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heartRed.svg");
            }
            else
            {
                var ProfileImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
                ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heartblue.svg");
            }




        }

        private void intialData()
        {
            Task.Run(async () =>
            {
                var result = await API.GetData<ResultDto<List<OfferDto>>>(helper.Constant.Urls.ApiPath, "Applicant/GetAllFavourteApplicantOffers",
                   await SecureStorage.GetAsync("AuthToken"));
                if (result.MessageCode == (int)MessageCodes.Success)
                {
                    List<OfferViewModel> list = new List<OfferViewModel>();
                    foreach (var item in result.Data)
                    {
                        var offerViewModel = new OfferViewModel()
                        {
                            Id = item.Id,
                            CompanyName = item.CompanyDto.Name ?? "",
                            JobTitle = "JobTitle : " + item.JobTitle ?? "",
                            CompanyEmail = item.CompanyEmail ?? "",

                            Description = item.JobDescription ?? "",
                            City = item.CompanyDto.CityDto.Name ?? "",
                            DateOfOffer = item.RegisterDate,
                            RegisterDate = helper.DayCalculate.GetTextOfDay(item.RegisterDate),
                            Image1 = getImageFromBase64(item.CompanyDto.Logo),
                            JobCategory = convertListToString(item.OfferJobCategoryDtos.Select(x => x.JobCategoryDto.Jobcategory).ToList())
                        };

                        list.Add(offerViewModel);
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (list != null && list.Count > 0)
                        {

                            offers = new ObservableCollection<OfferViewModel>(list.ToList());
                            OfferCollection.ItemsSource = offers;

                            OfferCollection.SetValue(IsVisibleProperty, true);
                            NoResult.SetValue(IsVisibleProperty, false);

                        }
                        else
                        {
                            OfferCollection.SetValue(IsVisibleProperty, false);
                            NoResult.SetValue(IsVisibleProperty, true);
                        }
                    });

                }

            });
        }

        private ImageSource getImageFromBase64(string image)
        {
            if (image == null) image = Domain.Constants.Images.CompanyImage.Replace("data:image/png;base64,", String.Empty);
            var imageBase64 = image.Replace("data: image/png;base64,", String.Empty);
            byte[] Base64Stream = Convert.FromBase64String(imageBase64);
            var imageSource = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            return imageSource;
        }
        private string convertListToString(List<string> list)
        {
            string text = "";
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    text += item;
                    if (item != list.LastOrDefault())
                        text += " and ";

                }
                return text;
            }
            return text;
        }

        private async void OnDeleteIconClicked(object sender, EventArgs e)
        {
            var ta = (TappedEventArgs)e;
            var parameter = ta.Parameter.ToString();

            if (await DisplayAlert(Res.Messages.Warning, Res.Messages.DeleteRecored, Res.Messages.Yes, Res.Messages.Cancel, FlowDirection.LeftToRight))
            {
                var result = await API.GetData<ResultDto<bool>>(helper.Constant.Urls.ApiPath, "Applicant/DeleteFavouriteOffer?id=" + parameter,
                   await SecureStorage.GetAsync("AuthToken"));

                if (result.MessageCode == (int)MessageCodes.Success)
                {
                    if (result.Data == true)
                    {
                        var token = await SecureStorage.GetAsync("AuthToken");
                        if (token != null)
                        {
                            var favouriteOfferResult = await API.GetData<ResultDto<List<ApplicantOffersFavouriteDto>>>(helper.Constant.Urls.ApiPath,
                                "Applicant/GetApplicantFavouriteOfferList", token);
                            if (favouriteOfferResult != null && favouriteOfferResult.Data.Count > 0)
                            {
                                var ProfileImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
                                ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heartRed.svg");

                            }
                            if (favouriteOfferResult != null && favouriteOfferResult.Data.Count == 0)
                            {
                                var ProfileImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
                                ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heartblue.svg");
                            }
                            intialData();
                        }
                    }
                }
            }
        }

        private async void onCollectionViewItemSelected(object sender, EventArgs e)
        {
            var re = (TappedEventArgs)e;
            var id = re.Parameter.ToString();

            await PopupNavigation.Instance.PushAsync(new ShowOfferPage(id));

        }
        protected override bool OnBackButtonPressed()
        {
            MainTheme.isVisible = true;
            Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                });
            });
            return true;
        }
        protected override void OnDisappearing()
        {
            MainTheme.isVisible = true;

            base.OnDisappearing();

        }
    }
}