using FFImageLoading.Svg.Forms;
using FindJobs.App.Themes;
using FindJobs.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyJobsPage : MainTheme
    {
        public MyJobsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            MainTheme.isVisible = false;
            var MyJobImage = (SvgCachedImage)GetTemplateChild("MyJobImage");
            MyJobImage.Source = AppResources.GetImageSvg("SharedIcons.paperblue.svg");
            base.OnAppearing();
        }
    }
}