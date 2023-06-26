using DotNek.AppComponents.Common;
using FindJobs.App.helper;
using System;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace FindJobs.App.Helper
{
    public class GetColorAppSetting: IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Source))
            {
                return null;
            }
            return Color.FromHex(GetUrl.Get(Source));
        }
    }
}
