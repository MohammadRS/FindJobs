using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowOfferPage : PopupPage
    {
         readonly string Id="";
        public ShowOfferPage(string id)
        {
            InitializeComponent();
            Id = id;
        }
        protected override void OnAppearing()
        {
            var url= helper.Constant.Urls.WebPath + "Home/Preview?Id=" + Id;
            ShowOfferDetailView.Source = url;

            ShowOfferDetailView.Navigated += async (o, s) =>
            {
              await ShowOfferDetailView.EvaluateJavaScriptAsync("document.getElementById('MainHeaderSite').style.display = 'none';");
            };
        }

        private async void onCloseButtonClicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}