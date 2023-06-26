using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FFImageLoading.Svg.Forms;
using FindJobs.App.helper;
using FindJobs.App.helper.Constant;
using FindJobs.App.Pages.ApplicantPages;
using FindJobs.App.Themes;
using FindJobs.Domain.Dtos;
using FindJobs.Resources;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplicantProfile : MainTheme
    {
        MemoryStream PhotoStream;
        public ApplicantProfile()
        {
            InitializeComponent();

           
        }
        protected override void OnAppearing()
        {
            MainTheme.isVisible= false;
            var ProfileImage = (SvgCachedImage)GetTemplateChild("ProfileImage");
            ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.profileblue.svg");

            LoadDataAsync();
            base.OnAppearing();
            Task.Run(async () =>
            {
                var token = await SecureStorage.GetAsync("AuthToken");
                var url = helper.Constant.Urls.WebPath + "Applicant/Profile?token=" + token;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ApplicantWebView.Source = url;


                    ApplicantWebView.Navigated += async (o, s) =>
                    {
                        await ApplicantWebView.EvaluateJavaScriptAsync("document.getElementById('MainHeaderSite').style.display = 'none';");
                    };

                });
            });
        }

        private  void LoadDataAsync()
        {
            Task.Run(async () =>
            {
                var token = await SecureStorage.GetAsync("AuthToken");
                var result = await API.GetData<ResultDto<ApplicantProfileDto>>(Urls.ApiPath, "Applicant/GetPersonalInformation", token);
                if (result.MessageCode == (int)MessageCodes.Success)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Load_Profile(result.Data);
                    });
                   
                }
            });
           
        }

        private void Load_Profile(ApplicantProfileDto data)
        {

            var imageSource = ImageSource.FromStream(() => GetImage(data.ApplicantImage));

            profileImage.Source = imageSource;

            fullName.Text = data.FirstName + " " + data.LastName;
            email.Text = data.Email;
        }

        private System.IO.Stream GetImage(string image)
        {
            var imageCut = image.Substring(23);
                var byteArray = Convert.FromBase64String(imageCut);
                return  new MemoryStream(byteArray);
        }

       

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonalInformation());
        }

        private async void ProfileDetail_Clicked(object sender, ClickedEventArgs e)
        {
            await Navigation.PushAsync(new ApplicantProfilePage());
        }

        private async void profileImage_Clicked(object sender, EventArgs e)
        {
            await CameraGallery.PickOrTakePhoto(this, async (images) =>
            {
                if (images.Count > 0)
                {
                    PhotoStream = new MemoryStream(images[0]);
                    var bytes = new byte[PhotoStream.Length];
                   await PhotoStream.ReadAsync(bytes, 0, (int)PhotoStream.Length);
                    string base64 = System.Convert.ToBase64String(bytes);
                    var token = await SecureStorage.GetAsync("AuthToken");
                    var result = await API.PostData<ResultDto<ApplicantProfileDto>>(Urls.ApiPath, "Applicant/UpdateApplicantImage",base64, token);
                    if (result.MessageCode == (int)MessageCodes.Success)
                    {
                        await DisplayAlert(Res.ApplicantProfile.Save, Res.ApplicantProfile.ImageUploadSuccessfully, Res.ApplicantProfile.ButtonOk);
                        profileImage.Source = ImageSource.FromStream(() => new MemoryStream(images[0]));
                    }

                   
                }
            });
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