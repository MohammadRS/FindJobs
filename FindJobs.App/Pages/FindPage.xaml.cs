using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FFImageLoading.Svg.Forms;
using FindJobs.App.Pages.Offers;
using FindJobs.App.Services.Implements;
using FindJobs.App.Themes;
using FindJobs.App.ViewModels;
using FindJobs.Domain.Dtos;
using FindJobs.Resources;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.IO;
using static Android.Media.Session.MediaSession;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindPage : MainTheme
    {
        List<OfferViewModel> offersObservableList = new List<OfferViewModel>();
        LoadData<GetOfferListMobile> offersData = new LoadData<GetOfferListMobile>();
        OffersFilter oldFilter = new OffersFilter();
        string cityname = "";
        string jobCategorylist = "";
        public FindPage(string cityName, string jobCategoryList)
        {
            cityname = cityName ?? "";
            jobCategorylist = jobCategoryList ?? "";
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            MainTheme.isVisible = false;
            var searchImage = (SvgCachedImage)GetTemplateChild("SearchImage");
            searchImage.Source = AppResources.GetImageSvg("SharedIcons.searchblue.svg");
            
            base.OnAppearing();
            
            initialData();
        }
        protected override  bool OnBackButtonPressed()
        {
            MainTheme.isVisible= true;
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

        private void initialData()
        {
            Domain.Dtos.OffersFilter offersFilter = new OffersFilter();

            if (!string.IsNullOrWhiteSpace(jobCategorylist) && jobCategorylist != null)
            {
                foreach (var item in jobCategorylist.Split(','))
                {
                    offersFilter.jobCategoryFilters.Add(new JobCategoryFilter
                    {
                        CategoryName = item
                    });
                }
            }

            Task.Run(async () =>
            {
                await GetOfferData(offersFilter, cityname, jobCategorylist);

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

        private void onFilterButtonClicked(object sender, EventArgs e)
        {
            var filterPage = new FilterOffersPage(oldFilter);

            filterPage.FilterEvent += (popupSender, offersFilter) =>
            {
                oldFilter = offersFilter;
                Task.Run(async () =>
                {
                    await GetOfferData(offersFilter, cityname, jobCategorylist);
                });

            };

            PopupNavigation.Instance.PushAsync(filterPage);
        }

        private async void onHeartIconCliced(object sender, EventArgs e)
        {
            var myImage = sender as SvgCachedImage;
            var ProfileImage = (SvgCachedImage)GetTemplateChild("SuggestImage");
            var token = await SecureStorage.GetAsync("AuthToken");

            bool isExist = false;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var te = (TappedEventArgs)e;
                string parameter = te.Parameter.ToString();
                var favouriteOfferResult = await API.GetData<ResultDto<List<ApplicantOffersFavouriteDto>>>(helper.Constant.Urls.ApiPath,
                       "Applicant/GetApplicantFavouriteOfferList", token);
                if (favouriteOfferResult != null && favouriteOfferResult.Data.Count > 0)
                {


                    foreach (var item in favouriteOfferResult.Data)
                    {
                        if (item.OfferId == int.Parse(parameter.ToString()))
                        {
                            isExist = true;
                            break;
                        }
                    }
                }
                if (isExist)
                {
                    var result = await API.GetData<ResultDto<bool>>(helper.Constant.Urls.ApiPath, "Applicant/DeleteFavouriteOffer?id=" + parameter,
                    await SecureStorage.GetAsync("AuthToken"));

                    if (result.MessageCode == (int)MessageCodes.Success)
                    {
                        if (result.Data)

                            if (myImage != null)
                            {
                                myImage.Source = AppResources.GetImageSvg("SharedIcons.heartblue.svg");
                            }
                        var IsHasRecord = await API.GetData<ResultDto<List<ApplicantOffersFavouriteDto>>>(helper.Constant.Urls.ApiPath,
                         "Applicant/GetApplicantFavouriteOfferList", token);
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (IsHasRecord != null && IsHasRecord.Data.Count == 0)
                            {
                                ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heart.svg");
                            }

                        });
                    }


                }
                else
                {
                    var reuslt = await API.GetData<ResultDto<ApplicantOffersFavouriteResult>>(helper.Constant.Urls.ApiPath, "Applicant/SaveApplicantOfferFavourite?OfferId=" + parameter,
                await SecureStorage.GetAsync("AuthToken"));
                    if (reuslt.Data == ApplicantOffersFavouriteResult.Success)
                    {
                        if (myImage != null)
                        {
                            myImage.Source = AppResources.GetImageSvg("SharedIcons.heartRed.svg");
                            ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.heartRed.svg");
                        }
                    }
                }

            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);
            }
        }

        private async void onCollectionViewItemSelected(object sender, EventArgs e)
        {
            var re = (TappedEventArgs)e;
            var id = re.Parameter.ToString();

            await PopupNavigation.Instance.PushAsync(new ShowOfferPage(id));

        }
        private async Task GetOfferData(OffersFilter offersFilter, string cityname, string jobCategorylist)
        {
            offersData.pageNumber = 1;
            var result = await offersData.GetFilteredOffers(offersFilter, "Company/GetOffersPaginationList", "", 20, cityname, jobCategorylist);

            if (result != null && result.Data.ItemsCount > 0)
            {
                var token = await SecureStorage.GetAsync("AuthToken");



                List<OfferViewModel> list = new List<OfferViewModel>();
                foreach (var item in result.Data.Data)
                {
                    var offerViewModel = new OfferViewModel()
                    {
                        Id = item.Id,
                        CompanyName = item.CompanyName ?? "",
                        JobTitle = "JobTitle : " + item.JobTitle ?? "",
                        CompanyEmail = item.CompanyEmail ?? "",
                        Description = item.Description ?? "",
                        City = item.City ?? "",
                        DateOfOffer = item.DateOfOffer,
                        RegisterDate = helper.DayCalculate.GetTextOfDay(item.DateOfOffer),
                        Image1 = getImageFromBase64(item.Logo),
                        JobCategory = convertListToString(item.JobCategoryNameList.ToList())
                    };
                    var favouriteOfferResult = await API.GetData<ResultDto<List<ApplicantOffersFavouriteDto>>>(helper.Constant.Urls.ApiPath,
                       "Applicant/GetApplicantFavouriteOfferList", token);
                    if (favouriteOfferResult != null && favouriteOfferResult.Data.Count > 0)
                    {
                        foreach (var offerFavouteId in favouriteOfferResult.Data)
                        {
                            if (item.Id == offerFavouteId.OfferId)
                            {
                                offerViewModel.FavouriteIcon = AppResources.GetImageSvg("SharedIcons.heartRed.svg");
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(offerViewModel.FavouriteIcon))
                    {
                        offerViewModel.FavouriteIcon = AppResources.GetImageSvg("SharedIcons.heartblue.svg");
                    }


                    list.Add(offerViewModel);




                }
                MainThread.BeginInvokeOnMainThread(() =>
                {


                    if (list != null && list.Count > 0)
                    {


                        offersObservableList = new List<OfferViewModel>(list.OrderByDescending(x => x.DateOfOffer).ToList());
                        OfferCollection.ItemsSource = offersObservableList;


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
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NoResult.SetValue(IsVisibleProperty, true);
                });
            }
        }
    }



}

