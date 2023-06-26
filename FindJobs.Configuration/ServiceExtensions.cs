using FindJobs.DataAccess.Entities;
using FindJobs.DataAccess.MapperProfiles;
using FindJobs.DataAccess.Persistence;
using FindJobs.DataAccess.Repositories;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using FindJobs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FindJobs.Configuration
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicantService, ApplicantService>();
            services.AddTransient<ISeedDataService, SeedDataService>();
            services.AddTransient<IMailSenderService, MailSenderService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJWTManagerService, JWTManagerService>();
            services.AddTransient<IRazorPartialToStringService, RazorPartialToStringService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IEncryption, Encryption>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPaymentService, PaymentService>();


            services.AddTransient<IGeoLocationService, GeoLocationService>();
            services.AddTransient<ICultureService, CultureService>();
            services.AddTransient<ICitiesService, CitiesService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICurrenciesService, CurrenciesService>();
            services.AddTransient<IJobCategoriService, JobCategoriesServices>();
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IGenericRepositorySimple<>), typeof(GenericRepositorySimple<>));
        }
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FindJobsDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
        public static void AddJwtAuthenticationToServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(configuration["AuthSettings:JwtKey"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["AuthSettings:JwtIssuer"],
                    ValidAudience = configuration["AuthSettings:JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }
        public static void RegisterMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApplicantProfile));
            services.AddAutoMapper(typeof(VerificationCodeProfile));
            services.AddAutoMapper(typeof(CountriesProfile));
            services.AddAutoMapper(typeof(CitiesProfile));
            services.AddAutoMapper(typeof(GeolocationProfile));
            services.AddAutoMapper(typeof(CulturesProfile));
            services.AddAutoMapper(typeof(ApplicantPreferenceProfile));
            services.AddAutoMapper(typeof(OfferProfile));
            services.AddAutoMapper(typeof(ApplicantDocumentsProfile));
            services.AddAutoMapper(typeof(CurrencyProfile));
            services.AddAutoMapper(typeof(LanguageProfile));
            services.AddAutoMapper(typeof(KnowledgeProfile));
            services.AddAutoMapper(typeof(PackageProfile));
            services.AddAutoMapper(typeof(OfferDocumentsProfile));
            services.AddAutoMapper(typeof(ApplicantOfferRequestProfile));
            services.AddAutoMapper(typeof(ApplicantOffersFavourite));
        }
    }
}