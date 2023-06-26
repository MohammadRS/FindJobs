using DotNek.AppComponents.Auth.FaceBook;
using DotNek.AppComponents.Auth.Google;
using DotNek.AppComponents.Common;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using UIKit;

namespace FindJobs.App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            if (Window == null)
            {
                Window = new UIWindow(frame: UIScreen.MainScreen.Bounds);
                var initialViewController = new SplashViewController();
                Window.RootViewController = initialViewController;
                Window.MakeKeyAndVisible();

                return true;
            }
            else
            {
                #region AddModule
                Rg.Plugins.Popup.Popup.Init();
                GoogleiOS.Init(this, AppSettings.Get("AuthSettings:GoogleAuthiOSClientId"));
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
                FaceBookiOS.Init(this,options);
                #endregion

                global::Xamarin.Forms.Forms.Init();
                CachedImageRenderer.Init();
                var ignore = typeof(SvgCachedImage);
                LoadApplication(new App());

                return base.FinishedLaunching(app, options);
            }
        }
    }
}
