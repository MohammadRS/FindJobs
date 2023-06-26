using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Services;
using FindJobs.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly IConfiguration configuration;

        public CompanyController(ICompanyService companyService, IConfiguration configuration)
        {
            this.companyService = companyService;
            this.configuration = configuration;
        }
        [HttpGet("SendOffers")]
        public async Task<IActionResult> SendOffers(string email)
        {
            //security issue: should not be able to send to any email
            var result = await companyService.SendOfferEmails(email);
            switch (result)
            {
                case EmailSendResult.Success:
                    var offers = await companyService.GetOffers();
                    return new JsonResult(new ResultDto<List<OfferDto>>(offers));
                case EmailSendResult.NotSend:
                    return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            }
            return new JsonResult("Not Send");
        }
        [HttpGet("GetOffer")]
        public async Task<IActionResult> GetOffer(int id)
        {
            var result = await companyService.GetOfferById(id);
            if (result != null)
                return new JsonResult(new ResultDto<OfferDto>(result));

            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }
        [HttpGet("GetTopEmployers")]
        public async Task<IActionResult> GetTopEmployers()
        {
            var topEmployers = await companyService.GetTopEmployers();
            return new JsonResult(new ResultDto<List<CompanyDto>>(topEmployers));
        }
        [HttpGet("CheckCompanyRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetRoleClaims()
        {
            //TODO: securiry issue
            if (companyService.GetCompanyRole(User))
                return new JsonResult(new ResultDto((int)MessageCodes.Success));
            else
                return new JsonResult(new ResultDto((int)MessageCodes.Unauthorized));
        }
        [HttpGet("GetOfferViewModel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOfferViewModel()
        {
            var email = GetCompanyEmail();
            var offerViewModel = await companyService.GetJobOfferViewModel(email);
            offerViewModel.JobCategories = await Domain.Global.Global.GetJobCategories(configuration["GlobalSettings:ApiUrl"]);
            return new JsonResult(new ResultDto<JobOfferViewModel>(offerViewModel));
        }
        [HttpGet("GetCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<CompanyDto> GetCompany()
        {
            var claims = User.Claims.ToList();
            var companyEmail = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return await companyService.GetCompanyByEmail(companyEmail);
        }
        private string GetCompanyEmail()
        {
            var claims = User.Claims.ToList();
            var companyEmail = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return companyEmail;
        }
        [HttpPost("SaveOrUpdateJobOffer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveOrUpdateJobOffer(OfferDto offerDto)
        {
            offerDto.CompanyEmail = GetCompanyEmail();
            var offerId = await companyService.SaveOrUpdateJobOffer(offerDto);
            if (offerId != 0)
            {
                return new JsonResult(new ResultDto<int>(offerId));

            }
            else
            {
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            }
        }

        [HttpPost("UpdateCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDto companyDto)
        {
            companyDto.Email = GetCompanyEmail();
            var result = await companyService.UpdateCompany(companyDto);
            return new JsonResult(new ResultDto<bool>(result));
        }

        [HttpGet("GetCompanyByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCompanyByEmail()
        {

            var result = await companyService.GetCompanyByEmail(GetCompanyEmail());
            return new JsonResult(new ResultDto<CompanyDto>(result));
        }

        [HttpPost("GetOffersPaginationList")]
       
        public IActionResult GetOffersPaginationList([FromBody] OffersFilter offerFilter=null)
        {
            var model = companyService.GetoffersByPagination(offerFilter);
            return new JsonResult(new ResultDto<PaginationDto<GetOfferListMobile>>(model));
        }

        [HttpGet("GetOffersFilter")]
        [ResponseCache(Duration =30000)]
        public IActionResult GetOffersFilter()
        {
            var model = companyService.GetOffersFilter(null);
            return new JsonResult(new ResultDto<OffersFilter>(model));
        }
        [HttpPost("CalculatateOfferFilter")]
        public IActionResult CalculatateOfferFilter( OffersFilter offersFilter)
        {
            var model=companyService.CalculateOfferDtoFilter(offersFilter);
            return new JsonResult(new ResultDto<OffersFilter>(model));
        }
        [HttpPost("GetOfferExample")]
        public IActionResult GetOfferExample()
        {
            var model = companyService.GetoffersByFilters(new OffersFilter());
            return new JsonResult(new ResultDto<List<OfferDto>>(model.ToList()));
        }
        
    }
}
