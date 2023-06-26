using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindJobs.DataAccess.Migrations
{
    public partial class SummarizeAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTop = table.Column<bool>(type: "bit", nullable: false),
                    CompanyRegistrationId = table.Column<int>(type: "int", nullable: false),
                    VatNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumberOfEmployee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPersonEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPersonPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AboutCompany = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CurrencyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrencySign = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Importance = table.Column<int>(type: "int", nullable: false),
                    ContinentCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhonePrefix = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsEuropeanUnion = table.Column<bool>(type: "bit", nullable: false),
                    Capital = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CurrencyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GeoLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromIp = table.Column<long>(type: "bigint", nullable: false),
                    ToIp = table.Column<long>(type: "bigint", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jobcategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Knowledges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knowledges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    Authority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    GetwayType = table.Column<int>(type: "int", maxLength: 15, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Done = table.Column<bool>(type: "bit", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRefunded = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifyCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    CreateVerifyTime = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyEmail = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benifits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false),
                    IsPartTime = table.Column<bool>(type: "bit", nullable: false),
                    Agreement = table.Column<bool>(type: "bit", nullable: false),
                    Freelancer = table.Column<bool>(type: "bit", nullable: false),
                    IsInternShip = table.Column<bool>(type: "bit", nullable: false),
                    IsWorkingFromHome = table.Column<bool>(type: "bit", nullable: false),
                    IsPartialWorkFromHome = table.Column<bool>(type: "bit", nullable: false),
                    IsWorkAtTheRegularWorkPlace = table.Column<bool>(type: "bit", nullable: false),
                    WorkPlaceAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTravelRequirement = table.Column<bool>(type: "bit", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpperLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerType = table.Column<int>(type: "int", nullable: false),
                    DoShowApplicantsSalary = table.Column<bool>(type: "bit", nullable: false),
                    HasPrimaryEducation = table.Column<bool>(type: "bit", nullable: false),
                    HasHighSchoolStudent = table.Column<bool>(type: "bit", nullable: false),
                    HasSecondarySchoolWithGraduation = table.Column<bool>(type: "bit", nullable: false),
                    HasSecondarySchoolWithOutGraduation = table.Column<bool>(type: "bit", nullable: false),
                    HasPostSecondary = table.Column<bool>(type: "bit", nullable: false),
                    HasUniversityStudent = table.Column<bool>(type: "bit", nullable: false),
                    HasHigherEducationLevel1 = table.Column<bool>(type: "bit", nullable: false),
                    HasHigherEducationLevel2 = table.Column<bool>(type: "bit", nullable: false),
                    HasHigherEducationLevel3 = table.Column<bool>(type: "bit", nullable: false),
                    HasDriverLicenceA = table.Column<bool>(type: "bit", nullable: false),
                    HasDriverLicenceB = table.Column<bool>(type: "bit", nullable: false),
                    HasDriverLicenceC = table.Column<bool>(type: "bit", nullable: false),
                    HasDriverLicenceD = table.Column<bool>(type: "bit", nullable: false),
                    HasDriverLicenceE = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExprationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Companies_CompanyEmail",
                        column: x => x.CompanyEmail,
                        principalTable: "Companies",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AsciiName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Lat = table.Column<double>(type: "float", maxLength: 100, nullable: false),
                    Lng = table.Column<double>(type: "float", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Iso3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Capital = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Population = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cultures_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferKnowledges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    KnowledgeId = table.Column<int>(type: "int", nullable: false),
                    KnowledgeLevel = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferKnowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferKnowledges_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    LanguageLevel = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferLanguages_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    IsDrivingLicense = table.Column<bool>(type: "bit", nullable: true),
                    IsEuropeanUnion = table.Column<bool>(type: "bit", nullable: true),
                    IsSwitzerland = table.Column<bool>(type: "bit", nullable: true),
                    IsUnitedStatesofAmerica = table.Column<bool>(type: "bit", nullable: true),
                    IsHourlyRate = table.Column<bool>(type: "bit", nullable: true),
                    HourlyAverage = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10, nullable: false),
                    HourlyFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HourlyUntil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsWorkPlace = table.Column<bool>(type: "bit", nullable: true),
                    IsWorkFromHome = table.Column<bool>(type: "bit", nullable: true),
                    IsPartialworkfromhome = table.Column<bool>(type: "bit", nullable: true),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: true),
                    IsPartTime = table.Column<bool>(type: "bit", nullable: true),
                    IsFreelancer = table.Column<bool>(type: "bit", nullable: true),
                    IsInternShip = table.Column<bool>(type: "bit", nullable: true),
                    RateType = table.Column<int>(type: "int", nullable: false),
                    LicenseType = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyToWorkStatus = table.Column<int>(type: "int", nullable: false),
                    ApplicantImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileVisible = table.Column<int>(type: "int", nullable: false),
                    AllowSearchEngines = table.Column<bool>(type: "bit", nullable: false),
                    ShowGender = table.Column<bool>(type: "bit", nullable: false),
                    ShowAge = table.Column<bool>(type: "bit", nullable: false),
                    ShowAddress = table.Column<bool>(type: "bit", nullable: false),
                    ShowPhone = table.Column<bool>(type: "bit", nullable: false),
                    ShowCountryOrCity = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Applicants_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionFile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantDocuments_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EducationLevel = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    InstituteName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    StartEducation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndEducation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantEducations_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantKnowledges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeLevel = table.Column<int>(type: "int", nullable: false),
                    KnowledgeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantKnowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantKnowledges_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LanguageLevel = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantLanguages_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantPreference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsSubscribed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantPreference_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantWorkExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    JobPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartWork = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndWork = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrentlyWorking = table.Column<bool>(type: "bit", nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantWorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantWorkExperiences_Applicants_ApplicantEmail",
                        column: x => x.ApplicantEmail,
                        principalTable: "Applicants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantDocuments_ApplicantEmail",
                table: "ApplicantDocuments",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantEducations_ApplicantEmail",
                table: "ApplicantEducations",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantKnowledges_ApplicantEmail",
                table: "ApplicantKnowledges",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantLanguages_ApplicantEmail",
                table: "ApplicantLanguages",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPreference_ApplicantEmail",
                table: "ApplicantPreference",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CityId",
                table: "Applicants",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CountryCode",
                table: "Applicants",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkExperiences_ApplicantEmail",
                table: "ApplicantWorkExperiences",
                column: "ApplicantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryCode",
                table: "Cities",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_CountryCode",
                table: "Cultures",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_OfferKnowledges_KnowledgeId",
                table: "OfferKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferKnowledges_OfferId",
                table: "OfferKnowledges",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferLanguages_LanguageId",
                table: "OfferLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferLanguages_OfferId",
                table: "OfferLanguages",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CompanyEmail",
                table: "Offers",
                column: "CompanyEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantDocuments");

            migrationBuilder.DropTable(
                name: "ApplicantEducations");

            migrationBuilder.DropTable(
                name: "ApplicantKnowledges");

            migrationBuilder.DropTable(
                name: "ApplicantLanguages");

            migrationBuilder.DropTable(
                name: "ApplicantPreference");

            migrationBuilder.DropTable(
                name: "ApplicantWorkExperiences");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "GeoLocation");

            migrationBuilder.DropTable(
                name: "JobCategories");

            migrationBuilder.DropTable(
                name: "OfferKnowledges");

            migrationBuilder.DropTable(
                name: "OfferLanguages");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Knowledges");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
