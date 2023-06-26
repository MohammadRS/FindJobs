using AutoMapper;
using DotNek.Utilities.Interfaces;
using FindJobs.DataAccess.Entities;
using FindJobs.DataAccess.Persistence;
using FindJobs.DataAccess.Repositories;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class SeedDataService : ISeedDataService
    {

        private readonly IConfiguration configuration;
        private readonly IZippingTool zippingTool;
        private readonly IConverterTool converterTool;
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;
        private FindJobsDbContext context;


        public SeedDataService(IConfiguration configuration, IZippingTool zippingTool,
            IConverterTool converterTool, IMapper mapper, IServiceProvider serviceProvider)
        {

            this.configuration = configuration;
            this.zippingTool = zippingTool;
            this.converterTool = converterTool;
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
        }
        public void AddSeedData()
        {
            Task.Run(async () =>
            {
                try
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        context = scope.ServiceProvider.GetRequiredService<FindJobsDbContext>();
                        await SeedJobCategory();
                        await SeedCurrency();
                        await SeedPackages();
                        await SeedLanguages();
                        await SeedKnowledges();
                        await SeedCountries();
                        await SeedCultures();
                        await SeedCities();
                        await SeedGeoLocations();
                    }
                }
                catch (Exception)
                {
                }
            });
        }
        private async Task SeedCurrency()
        {
            var currencyTypeGenericRepository = new GenericRepositorySimple<Currency>(context);
            var table = currencyTypeGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:CurrenciesPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<CurrencyDto>(builder.ToString());
            var currencies = mapper.Map<List<Currency>>(data);
            currencyTypeGenericRepository.AddEntityRange(currencies);
        }

        private async Task SeedLanguages()
        {
            var LanguageGenericRepository = new GenericRepositorySimple<Language>(context);
            var table = LanguageGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:LanguagesPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<LanguageDto>(builder.ToString());
            var languages = mapper.Map<List<Language>>(data);
            LanguageGenericRepository.AddEntityRange(languages);
        }
        private async Task SeedPackages()
        {
            var packageGenericRepository = new GenericRepositorySimple<Package>(context);
            var table = packageGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:PackagesPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<PackageDto>(builder.ToString());
            var publicationAmounts = mapper.Map<List<Package>>(data);
            packageGenericRepository.AddEntityRange(publicationAmounts);
        }
        private async Task SeedKnowledges()
        {
            var KnowledgeGenericRepository = new GenericRepositorySimple<Knowledge>(context);
            var table = KnowledgeGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:KnowledgesPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<KnowledgeDto>(builder.ToString());
            var knowledges = mapper.Map<List<Knowledge>>(data);
            KnowledgeGenericRepository.AddEntityRange(knowledges);
        }

        private async Task SeedJobCategory()
        {
            var JobCategoryGenericRepository = new GenericRepositorySimple<JobCategory>(context);
            var table = JobCategoryGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:JobCategoryPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            try
            {
                var data = converterTool.CsvToList<JobCategoryDto>(builder.ToString());
                var JobCategoriesList = mapper.Map<List<JobCategory>>(data);
                JobCategoryGenericRepository.AddEntityRange(JobCategoriesList);
            }
            catch (NullReferenceException e)
            {

                Console.WriteLine(e);
            }
        }

        private async Task SeedCountries()
        {
            var countryGenericRepository = new GenericRepositorySimple<Country>(context);
            var table = countryGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:CountriesListPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<CountryDto>(builder.ToString());
            var countriesList = mapper.Map<List<Country>>(data);
            countryGenericRepository.AddEntityRange(countriesList);
        }

        public async Task SeedCities()
        {
            var cityGenericRepository = new GenericRepositorySimple<City>(context);
            var table = cityGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:CitiesListPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<CityDto>(builder.ToString());
            var citiesList = mapper.Map<List<City>>(data);
            cityGenericRepository.AddEntityRange(citiesList);
        }
        public async Task SeedCultures()
        {
            try
            {
                var cultureGenericRepository = new GenericRepositorySimple<Culture>(context);
                var table = cultureGenericRepository.GetEntities().Any();
                if (table) return;
                var file = await File.ReadAllBytesAsync(configuration["SeedSettings:CulturesListPath"]);
                var extractedFiles = await zippingTool.ExtractZipFile(file);
                var extractedFile = extractedFiles[0];
                var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
                var data = converterTool.CsvToList<SeedDataCultureDto>(builder.ToString());
                var cultureList = mapper.Map<List<Culture>>(data);
                cultureGenericRepository.AddEntityRange(cultureList);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task SeedGeoLocations()
        {
            var geoLocationGenericRepository = new GenericRepositorySimple<GeoLocation>(context);
            var table = geoLocationGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:ServerListPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<GeoLocationDto>(builder.ToString());
            var geoLocationList = mapper.Map<List<GeoLocation>>(data);
            geoLocationGenericRepository.AddEntityRange(geoLocationList);
        }

        public async Task SeedCompany()
        {
            var companyGenericRepository = new GenericRepositorySimple<Company>(context);
            var table = companyGenericRepository.GetEntities().Any();
            if (table) return;
            var file = await File.ReadAllBytesAsync(configuration["SeedSettings:CompanyListPath"]);
            var extractedFiles = await zippingTool.ExtractZipFile(file);
            var extractedFile = extractedFiles[0];
            var builder = await converterTool.ByteArrayToStringBuilder(extractedFile.File);
            var data = converterTool.CsvToList<CompanyDto>(builder.ToString());
            var companyList = new List<CompanyDto>();
            foreach (var item in data)
            {
                item.Logo = item.Logo.Replace("base64", "base64,");
                companyList.Add(item);
            }
            var geoLocationList = mapper.Map<List<Company>>(companyList);
            companyGenericRepository.AddEntityRange(geoLocationList);
        }
    }
}
