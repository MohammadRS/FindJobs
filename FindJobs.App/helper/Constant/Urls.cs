using System;
using System.Collections.Generic;
using System.Text;

namespace FindJobs.App.helper.Constant
{
    public static class Urls
    {
        public static readonly string ApiPath = GetUrl.Get("GlobalSettings:webApiUrl");
        public static readonly string WebPath = GetUrl.Get("GlobalSettings:WebUrl");
    }
}
