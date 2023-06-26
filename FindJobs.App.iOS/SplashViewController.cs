using UIKit;

namespace FindJobs.App.iOS
{
    public partial class SplashViewController : UIViewController
    {
        public SplashViewController() : base()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var viewImage = new UIImageView
            {
                Image = UIImage.FromBundle("splash_logo.png"),
                ContentMode = UIViewContentMode.Center
            };
            if (View == null) return;
            View.AddSubview(viewImage);
            View.BackgroundColor = UIColor.White;
            viewImage.Center = View.Center;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}