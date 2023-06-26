using FFImageLoading.Svg.Forms;
using FindJobs.App.Themes;
using FindJobs.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobAlertPage : MainTheme
    {
        public JobAlertPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MainTheme.isVisible = false;
            var JobAlertImage = (SvgCachedImage)GetTemplateChild("JobAlertImage");
            JobAlertImage.Source = AppResources.GetImageSvg("SharedIcons.bellblue.svg");
            base.OnAppearing();
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