using FindJobs.DataAccess.Entities;
using FindJobs.DataAccess.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FindJobs.DataAccess.Persistence
{
    public class FindJobsDbContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<ApplicantPreference> ApplicantPreferences { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<ApplicantEducation> ApplicantEducations { get; set; }
        public DbSet<ApplicantKnowledge> ApplicantKnowledges { get; set; }
        public DbSet<ApplicantLanguage> ApplicantLanguages { get; set; }
        public DbSet<ApplicantWorkExperience> ApplicantWorkExperiences { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<ApplicantDocuments> ApplicantDocuments { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<OfferKnowledge> OfferKnowledges { get; set; }
        public DbSet<OfferLanguage> OfferLanguages { get; set; }
        public DbSet<OfferJobCategory> OfferJobCategories { get; set; }
        public DbSet<OfferDocuments> OfferDocuments { get; set; }
        public DbSet<ApplicantOfferRequest> ApplicantOfferRequests { get; set; }
        public DbSet<ApplicantOffersFavourite> ApplicantOffersFavourites { get; set; }
        public FindJobsDbContext(DbContextOptions<FindJobsDbContext> options) : base(options)
        {

            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CountriesMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
