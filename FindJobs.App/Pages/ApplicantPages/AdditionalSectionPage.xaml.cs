using FindJobs.App.Themes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages.ApplicantPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalSectionPage : MainTheme
    {
        public AdditionalSectionPage()
        {
            InitializeComponent();
        }

        private void ChboxDriving_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (chboxdriving.IsChecked)
                licnesbox.IsVisible = true;
            else
                licnesbox.IsVisible = false;
        }

        private void Chboxrate_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(chboxEnterRange.IsChecked)
            {
                rateboxnotrenged.IsVisible = false;
                rateboxrenged.IsVisible = true;
            }
            else
            {
                rateboxnotrenged.IsVisible = true;
                rateboxrenged.IsVisible = false;
            }

        }
    }
}