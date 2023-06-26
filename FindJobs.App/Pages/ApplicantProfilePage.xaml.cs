using FindJobs.App.helper;
using FindJobs.App.Themes;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplicantProfilePage : MainTheme
    {
        public ApplicantProfilePage()
        {
            InitializeComponent();
            MainTheme.isVisible = false;
        }

        private async void Contact_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantContactInfo();
        }

        private async void Upload_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantUploadDocuments();
        }

        private async void WorkExperience_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantWorkExperince();
        }

        private async void Education_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantWorkEducation();
        }

        private async void Knowledge_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantKnowledge(); 
        }

        private async void Language_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantLanguage();
        }

        private async void AdditionalSection_Clicked(object sender, ClickedEventArgs e)
        {
            await Nav.ApplicantAdditional();
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