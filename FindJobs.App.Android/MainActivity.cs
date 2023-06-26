using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using DotNek.AppComponents.Auth.ReCaptcha;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using DotNek.AppComponents.Auth.FaceBook;
using Android.Content;
using DotNek.AppComponents.Common;
using DotNek.AppComponents.Auth.Google;
using FindJobs.App.helper;
namespace FindJobs.App.Droid
{
    [Activity(Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            #region AddModule
            GoogleAndroid.Init(this, AppSettings.Get("AuthSettings:GoogleAuthAndroidClientId"));
            FaceBookAndroid.Init(this, AppSettings.Get("AuthSettings:FacebookAppId"), AppSettings.Get("AuthSettings:FacebookAppClientToken"));
            ReCaptchaAndroid.Init(this);
            Rg.Plugins.Popup.Popup.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            #endregion

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            LoadApplication(new App());
        }
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            GoogleAndroid.LoginCallBack(requestCode, resultCode, data);
            FaceBookAndroid.LoginCallBack(requestCode, resultCode, data);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}