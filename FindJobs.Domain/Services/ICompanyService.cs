using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface ICompanyService
    {
        Task<EmailSendResult> SendOfferEmails(string email);
        Task<OfferDto> GetOfferById(int id);
        Task<List<OfferDto>> GetOffers();
        Task<List<CompanyDto>> GetTopEmployers();
        Task<bool> UpdateCompany(CompanyDto companyDto);
        bool GetCompanyRole(ClaimsPrincipal user);
        Task<JobOfferViewModel> GetJobOfferViewModel(string email);
        Task<int> SaveOrUpdateJobOffer(OfferDto offerDto);
        Task<CompanyDto> GetCompanyByEmail(string email);

        PaginationDto<GetOfferListMobile> GetoffersByPagination(OffersFilter offersFilter=null);

        IQueryable<OfferDto> GetoffersByFilters(OffersFilter offersFilter = null);
        OffersFilter GetOffersFilter(List<OfferDto> offerDtos = null);
        OffersFilter CalculateOfferDtoFilter(OffersFilter offersFilter);

    }
}
