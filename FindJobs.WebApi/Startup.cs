using DotNek.Utilities;
using DotNek.WebComponents.Areas.Middlewares.Exceptions;
using FindJobs.Configuration;
using FindJobs.Domain.Services;
using FindJobs.WebApi.CustomeMiddleWares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FindJobs.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            #region FindJobs.Configuration 
            services.AddDatabaseContext(Configuration);
            services.RegisterRepositories();
            services.RegisterServices();
            services.RegisterMapperProfiles();
            services.AddJwtAuthenticationToServices(Configuration);
            BootStrapper.Configuration(services);
            #endregion



            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindJobs.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedDataService seedDataService)
        {
            seedDataService.AddSeedData();

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();



            app.UseStaticFiles();
            app.UseUnauthorizedMiddlware();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
