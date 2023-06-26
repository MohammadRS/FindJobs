using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SkiaSharp;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace FindJobs.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration configuration;

        public BaseController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cul = filterContext.RouteData.Values["culture"].ToString();
            var culture = (filterContext.RouteData.Values["culture"] ?? new CultureDto().CultureCode).ToString();
            ViewData["Culture"] = culture;
            ChangeThreadCulture(culture);
            var cultureList = Domain.Global.Global.GetCultureList(configuration["GlobalSettings:ApiUrl"]).Result;
            ViewData["CultureList"] = cultureList;
            var countryList = Domain.Global.Global.GetCountryList(configuration["GlobalSettings:ApiUrl"]).Result;
            ViewData["CountryList"] = countryList;

            var cultureDto = culture is not null
                ? cultureList.FirstOrDefault(x =>
                    String.Equals(x.CultureCode, culture, StringComparison.CurrentCultureIgnoreCase))
                : new CultureDto();
            ViewData["CurrentCultureDto"] = cultureDto;
            ViewData["ProjectName"] = configuration["GlobalSettings:ProjectName"];
            ViewData["ColorTeal"] = configuration["UISettings:ColorTeal"];
            ViewData["ColorAqua"] = configuration["UISettings:ColorAqua"];
            ViewData["HeaderColorDark"] = ViewData["ColorTeal"];
            ViewData["HeaderColorLight"] = ViewData["ColorAqua"];
            ViewData["ColorWhite"] = configuration["UISettings:ColorWhite"];
            ViewData["ColorGray"] = configuration["UISettings:ColorGray"];
            ViewData["ColorLightGray"] = configuration["UISettings:ColorLightGray"];
            ViewData["DarkBlue"] = configuration["UISettings:DarkBlue"];
            ViewData["Red"] = configuration["UISettings:Red"];

            ViewData["AuthModel"] = new AuthConfiguration()
            {
                ColorTeal = ViewData["ColorTeal"].ToString(),
                ColorAqua = ViewData["ColorAqua"].ToString(),
                TermsLink = configuration["AuthSettings:TermsLink"],
                GoogleAuthClientId = configuration["AuthSettings:GoogleAuthClientId"],
                FacebookAppId = configuration["AuthSettings:FacebookAppId"],
                CaptchaSiteKey = configuration["GlobalSettings:GoogleCaptchaSiteKey"],
                IsCaptchaVisible = false,
                Culture = culture
            };
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                if (HttpContext.Session.Get("ApplicantImage") == null)
                {
                    var AppliantImage = GetApplicantImage(token);
                    if (AppliantImage != null)
                        HttpContext.Session.SetString("ApplicantImage", AppliantImage);
                }
            }
            //if (!string.IsNullOrWhiteSpace(token) && !IsApplicantExist(token))
            //{
            //    foreach (var cookie in HttpContext.Request.Cookies)
            //    {
            //        Response.Cookies.Delete("AuthToken");
            //    }

            //}
            ViewData["AuthData"] = new AppResources().GetViewData(culture, "Auth");

            //TODO Check this part for optimaztion

            //    if (cultureDto.CountryCode == "US" || cultureDto.CountryCode == "CA")
            //    {
            //        ViewData["Regions"] = Startup.Cities.Where(x => x.CountryCode.ToLower() ==
            //                        cultureDto.CountryCode.ToLower()).Distinct().ToList();

            //    }
            //    else
            //    {
            //        ViewData["Regions"] = Startup.Cities.Where(x => x.CountryCode.ToLower() ==
            //                                   cultureDto.CountryCode.ToLower()).OrderByDescending(x => x.Population).Take(10).ToList();
            //    }
            
            //ViewData["JobCategoy"] = Startup.JobCategories.ToList();
        }
        private void ChangeThreadCulture(string culture)
        {
            var newCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

        }
        public static string PlaceholderImage(int width, int height)
        {
            byte[] byteArray;
            var stream = new MemoryStream();
            if (width == 0 || height == 0)
            {
                byteArray = new byte[] { };
            }
            else
            {
                if (width > 50 || height > 50)
                {
                    width = (int)Math.Round((double)width / 10, 0);
                    height = (int)Math.Round((double)height / 10, 0);
                }
                var info = new SKImageInfo(width, height);
                var surface = SKSurface.Create(info);
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);
                var image = surface.Snapshot();
                var data = image.Encode(SKEncodedImageFormat.Png, 100);
                data.SaveTo(stream);
            }
            byteArray = stream.ToArray();
            return "data:image/png;base64, " + Convert.ToBase64String(byteArray);
        }
        public string GetApplicantImage(string token)
        {
            var WebApiUrl = configuration["GlobalSettings:ApiUrl"];
            var result = (API.GetData<ResultDto<string>>
               (WebApiUrl, "Applicant/GetApplicantImage", token).Result).Data;
            return result;
        }



    }
}