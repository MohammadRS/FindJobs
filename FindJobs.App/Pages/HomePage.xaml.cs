using FindJobs.App.helper;
using FindJobs.App.Themes;
using System;
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
    public partial class HomePage : MainTheme
    {
        public HomePage()
        {
            InitializeComponent();
          
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (ResponsiveLayout != null)
            {
                if (width > 960)
                    ResponsiveLayout.Orientation = StackOrientation.Horizontal;
                else
                    ResponsiveLayout.Orientation = StackOrientation.Vertical;
            }
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