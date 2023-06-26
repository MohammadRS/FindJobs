using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Threading.Tasks;

namespace FindJobs.App.Droid
{
    [Activity(MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await SimulateStartup();
        }

        private async Task SimulateStartup()
        {
           // await Task.Delay(TimeSpan.FromSeconds(3));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}