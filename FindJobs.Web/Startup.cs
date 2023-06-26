using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Middlewares.Exceptions;
using FindJobs.Domain.Dtos;
using FindJobs.Web.Global;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebOptimizer;
using System.Globalization;
using System.Threading;

namespace FindJobs.Web
{
    public class Startup
    {
        public static Dictionary<string, string> InlineStyles { get; set; }

        public static List<string> ContinentGroups = Domain.Global.Global.continentGroups;

        public static List<CityDto> Cities = new List<CityDto>();
        public static List<JobCategoryDto> JobCategories = new List<JobCategoryDto>();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddProgressiveWebApp();
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.FallBackToParentUICultures = true;
                options.RequestCultureProviders.Clear();
            });

            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();


            services.AddHttpContextAccessor();

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.MinifyJsFiles("js/*.js");
                pipeline.AddJavaScriptBundle("/js/layout-lazy.js", "js/layout-lazy/*.js");

                pipeline.MinifyCssFiles("css/*.css");
                pipeline.AddCssBundle("/css/layout-lazy.css", "css/layout-lazy/*.css");
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            //........
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
            services.AddTransient<IGlobalResources, GlobalResources>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
            app.UseRewriter(new RewriteOptions().AddRewrite("sw.js", "pwa/sw.js", true));

            app.UseRequestLocalization();
            var globalResource = new GlobalResources(env, Configuration);
            InlineStyles= globalResource.GetInlineStyles();


           // InlineStyles = globalResources.GetInlineStyles();

            Domain.Global.Global.GetCultureList(Configuration["GlobalSettings:ApiUrl"]).ConfigureAwait(false);
            Cities = (GetAllCities().Result);
            JobCategories = (GetJobCategory().Result);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            var currentDirectory = Directory.GetCurrentDirectory();
            app.UseWebOptimizer(env, new[]
            {
                new FileProviderOptions
                {
                    RequestPath = "/css/",
                    FileProvider = new PhysicalFileProvider(Path.Combine(currentDirectory, "wwwroot/css"))
                },
                new FileProviderOptions
                {
                    RequestPath = "/js/",
                    FileProvider = new PhysicalFileProvider(Path.Combine(currentDirectory, "wwwroot/js"))
                }
            });
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseSession();
            app.UseMiddleware<RequestLocalizationMiddleware>();


           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture}/{controller=Home}/{action=Index}/{id?}",
                    constraints: new { culture = "[a-z]{2}-[A-Z]{2}" }
                    );

                endpoints.MapRazorPages();

                endpoints.MapFallback(async context =>
                {

                    var culture = "";
                    var cul = context.GetRouteData().Values["path"] ?? "";
                    if (!string.IsNullOrWhiteSpace(cul.ToString()))
                    {

                        int index = cul.ToString().IndexOf("/");
                        culture = cul.ToString().Substring(0, index);
                    }
                    if (culture == "")
                    {

                        var url = context.Request.Path;
                        var cultureDto = await GetCultureByIp(context.Request.HttpContext);
                        CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        context.Response.Redirect("/" + cultureDto.CultureCode + url);
                    }
                    else
                    {

                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        context.Response.Redirect("/" + culture + "/Errors/Page404");

                    }

                });
            });
        }

        private async Task<List<CityDto>> GetAllCities()
        {
            return
               (await API.GetData<ResultDto<List<CityDto>>>
               (Configuration["GlobalSettings:ApiUrl"],
                "Culture/GetCities")).Data;
        }

        private async Task<List<JobCategoryDto>> GetJobCategory()
        {
            return
             (await API.GetData<ResultDto<List<JobCategoryDto>>>
            (Configuration["GlobalSettings:ApiUrl"],
            "Shared/GetJobCategories")).Data;

        }
        public static string GetStringContinent(string continent)
        {
            return continent switch
            {
                "NA" => Res.Layout.NA,
                "EU" => Res.Layout.EU,
                "SA" => Res.Layout.SA,
                "AS,OC,AF" => Res.Layout.AS_OC_AF,
                _ => throw new ArgumentOutOfRangeException(nameof(continent), continent, null)
            };
        }
        private async Task<CultureDto> GetCultureByIp(HttpContext httpContext)
        {
            var ip = httpContext?.Connection.RemoteIpAddress?.ToString();
            var browserLanguages = httpContext?.Request.GetTypedHeaders()
                .AcceptLanguage
                ?.OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToArray() ?? Array.Empty<string>();

            var cultureViewModel = new CultureResponseDto()
            {
                Ip = ip,
                BrowserLanguage = browserLanguages
            };
            var culture = await API.PostData<ResultDto<CultureDto>>(Configuration["GlobalSettings:ApiUrl"],
                "Culture/GetCultureByIp", cultureViewModel);
            return culture.Data;
        }

    }
}