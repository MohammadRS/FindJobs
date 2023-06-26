using DotNek.AppComponents.Auth.FaceBook;
using DotNek.AppComponents.Common;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNek.AppComponents.Auth.Google;
using Xamarin.Essentials;
using Xamarin.Forms;
using DotNek.AppComponents.Auth.ReCaptcha;
using Res;
using DotNek.AppComponents.Power;
using FindJobs.Domain.Enums;
using FindJobs.Domain.ViewModels;
using Xamarin.Forms.Xaml;
using PlatformType = FindJobs.Domain.Enums.PlatformType;
using FindJobs.App.helper;
using FindJobs.App.helper.Constant;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : PopupPage
    {
        Action callBackAction;
        private static readonly string SiteKey = DeviceInfo.Platform == DevicePlatform.Android ? AppSettings.Get("AuthSettings:AndroidRecaptchaKey") : AppSettings.Get("AuthSettings:IosRecaptchaKey");
        private static readonly string BaseApiUrl = AppSettings.Get("GlobalSettings:webApiUrl");
        public static readonly int ApplicantType = (int)ClientTypes.Applicant;
        public static readonly int CompanyType = (int)ClientTypes.Company;
        public VerifyCodeViewModel VerifyCodeViewModel { get; set; }

        public LoginPage(Action callBackAction)
        {
            InitializeComponent();
            VerifyCodeViewModel = new VerifyCodeViewModel();
            this.callBackAction = callBackAction;
            ErrorCheckLabel.TextColor = Color.FromHex(AppSettings.Get("UISettings:ColorRed"));
            ErrorCheckLabelCompany.TextColor = Color.FromHex(AppSettings.Get("UISettings:ColorRed"));
            SendCodeButton.BackgroundColor = Color.FromHex(AppSettings.Get("UISettings:ColorTeal"));
            SendCodeButtonCompany.BackgroundColor = Color.FromHex(AppSettings.Get("UISettings:ColorTeal"));
        }
         

        private async void CloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
        protected override  bool OnBackButtonPressed()
        {
             Navigation.PopPopupAsync();
            return false;
        }
        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void SendCode_Clicked(object sender, EventArgs e)
        {
            VerifyCodeViewModel.Type = ApplicantType.ToString();
            SendCode(ErrorCheckLabel, SendCodeButton, CodeEntry, SignInButton
           , EmailEntry);
        }
        private async void LoginWithEmailClicked(object sender, EventArgs e)
        {
            VerifyCodeViewModel.Type = ApplicantType.ToString();
            await SignIn(ErrorCheckLabel.Text, EmailEntry.Text, CodeEntry.Text);
        }


        private void SendCodeCompany_Clicked(object sender, EventArgs e)
        {
            VerifyCodeViewModel.Type = CompanyType.ToString();
            SendCode(ErrorCheckLabelCompany, SendCodeButtonCompany, CodeEntryCompany, SignInButtonCompany
            , EmailEntryCompany);
        }

        private async void LoginWithEmailCompanyClicked(object sender, EventArgs e)
        {
            VerifyCodeViewModel.Type = CompanyType.ToString();
            await SignIn(ErrorCheckLabelCompany.Text, EmailEntryCompany.Text, CodeEntryCompany.Text);
        }
        public async void SendCode(Label errorCheckLabel, Button sendCodeButton, PowerEntry codeEntry,
            Button signInButton, PowerEntry emailEntry)
        {
            VerifyCodeViewModel.Email = emailEntry.Text;
            if (string.IsNullOrEmpty(VerifyCodeViewModel.Email))
            {
                errorCheckLabel.TextColor = Color.FromHex(AppSettings.Get("UISettings:ColorRed"));
                errorCheckLabel.Text = Auth.VerifyCodeNotValid;
                return;
            }
            VerifyCodeViewModel.Email = emailEntry.Text.Trim();
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(VerifyCodeViewModel.Email);
            if (!match.Success)
            {
                errorCheckLabel.Text = Auth.EmailNotValid;
                return;
            }
            sendCodeButton.IsEnabled = false;
            var token = await ReCaptcha.Current.Verify(SiteKey, BaseApiUrl);
            if (token == null)
            {
                sendCodeButton.IsEnabled = true;
                return;
            }
            VerifyCodeViewModel.Captcha = token;
          
            var response =await  API.GetData<ResultDto>(Urls.ApiPath , "auth/SendVerificationCode?email=" + VerifyCodeViewModel.Email + "&captcha=" + VerifyCodeViewModel.Captcha + "&ApplicantType=" + VerifyCodeViewModel.Type + "&platformType=" + PlatformType.Android);
            if (response is { MessageCode: 0 })
            {
                codeEntry.IsEnabled = true;
                signInButton.IsEnabled = true;
                int t = 60;
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    t -= 1;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        sendCodeButton.Text = t < 10 ? $"00:0{t}" : $"00:{t}";
                        if (t == 0) sendCodeButton.Text = Auth.SendCode;
                    });
                    if (t >= 1) return true;
                    sendCodeButton.IsEnabled = true;
                    return false;
                });
            }
            else
            {
                sendCodeButton.IsEnabled = true;
            }
        }

        public async Task SignIn(string errorCheckLabelText, string emailEntryText, string codeEntryText)
        {
            VerifyCodeViewModel.Email = emailEntryText;
            if (string.IsNullOrEmpty(VerifyCodeViewModel.Email))
            {
                errorCheckLabelText = Auth.EmailNotValid;
                return;
            }
            VerifyCodeViewModel.Code = codeEntryText;
            if (string.IsNullOrEmpty(VerifyCodeViewModel.Code))
            {
                errorCheckLabelText = Auth.VerifyCodeNotValid;
                return;
            }
            var response = await API.GetData<ResultDto<TokenResDto>>(Urls.ApiPath, "auth/SignInWithVerificationCode?email=" + VerifyCodeViewModel.Email + "&verificationCode=" + VerifyCodeViewModel.Code + "&type=" + VerifyCodeViewModel.Type);
            switch (response.Data.MessageCode)
            {
                case MessageCodes.Success:
                    {
                        await SecureStorage.SetAsync("AuthToken", response.Data.Token);
                        await SecureStorage.SetAsync("LoginType",VerifyCodeViewModel.Type.ToString());
                        await Navigation.PopPopupAsync();
                        if (VerifyCodeViewModel.Type.Equals(ApplicantType.ToString()))
                        {
                           await Application.Current.MainPage.Navigation.PushAsync(new ApplicantProfile());
                        }
                        else if (VerifyCodeViewModel.Type.Equals(CompanyType.ToString()))
                        {
                           await Application.Current.MainPage.Navigation.PushAsync(new CompanyProfile());
                        }

                        break;
                    }
                case MessageCodes.VerificationCodeNotValid:
                    errorCheckLabelText = Auth.VerifyCodeNotValid;
                    break;
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                });
            });
        }
        public async Task ProcessLoginWithFacebook(string FacebookToken)
        {

           
            var result = await API.PostData<ResultDto<TokenResDto>>(Urls.ApiPath, "Auth/SignInWithFacebook", FacebookToken);
            if (result != null)
            {
                if (result.MessageCode == (int)MessageCodes.Success)
                {
                    await SecureStorage.SetAsync("AuthToken", result.Data.Token);
                    await Navigation.PopPopupAsync();
                  await Navigation.PushAsync( new ApplicantProfile());
                }

            }

        }
        private void LoginWithFacebookClicked(object sender, ClickedEventArgs e)
        {
            FaceBook.OnSuccess = async (FacebookToken) =>
            {
                if (!string.IsNullOrWhiteSpace(FacebookToken))
                    await ProcessLoginWithFacebook(FacebookToken);
                callBackAction();
            };
            FaceBook.OnError = (Error) =>
            {
            };
            FaceBook.OnCancel = () =>
            {
            };
            FaceBook.Current.Login();
        }

        private void LoginWithGoogleClicked(object sender, ClickedEventArgs e)
        {
            Google.OnSuccess = async (GoogleToken) =>
            {
                if (!string.IsNullOrWhiteSpace(GoogleToken))
                {
                    var result = await API.GetData<ResultDto<TokenResDto>>(Urls.ApiPath, "Auth/SignInWithGoogle?token=" + GoogleToken);
                  
                    if (result != null)
                    {
                       
                        if (result.MessageCode == (int)MessageCodes.Success) //TODO: We already have enum for this
                        {
                            await SecureStorage.SetAsync("AuthToken", result.Data.Token);
                            await Navigation.PopPopupAsync();
                           await Application.Current.MainPage.Navigation.PushAsync(new ApplicantProfile());
                        }

                    }
                }
            };
            Google.OnError = (Error) =>
            {
            };
            Google.OnCancel = () =>
            {
            };
            Google.Current.Login();
        }
    }
}