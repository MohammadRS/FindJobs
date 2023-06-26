using FindJobs.App.helper.Constant;
using FindJobs.App.Pages;
using FindJobs.Domain.Global;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindJobs.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Startup

            MainPage = new NavigationPage(new HomePage());

          //MainPage = new NavigationPage(new ApplicantProfilePage());
        }

        protected override void OnStart()
        {
            Task.Run(async () =>
            {
                await Global.GetCityList(Urls.ApiPath);
                await Global.GetCultureList(Urls.ApiPath);
                await Global.GetJobCategories(Urls.ApiPath);
            });
         
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
