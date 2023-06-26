using FFImageLoading.Svg.Forms;
using FindJobs.App.Themes;
using FindJobs.Resources;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanyProfile : MainTheme
    {
        public CompanyProfile()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            var ProfileImage = (SvgCachedImage)GetTemplateChild("ProfileImage");
            ProfileImage.Source = AppResources.GetImageSvg("SharedIcons.profileblue.svg");
            MainTheme.isVisible= false;
            initialData();
            base.OnAppearing();
           
        }
        private  void initialData()
        {
            Task.Run(async () =>
            {
                var token = await SecureStorage.GetAsync("AuthToken");
                var url = helper.Constant.Urls.WebPath + "Company/Profile?token=" + token;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CompanyWebView.Source = url;
                  

                    CompanyWebView.Navigated += async (o, s) =>
                    {
                        await CompanyWebView.EvaluateJavaScriptAsync("document.getElementById('MainHeaderSite').style.display = 'none';");
                    };
                });
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