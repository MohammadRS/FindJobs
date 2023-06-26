using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Enums;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.ViewModels;
using FindJobs.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SkiaSharp;
using System;
using System.Threading.Tasks;

namespace FindJobs.Web.Controllers
{
    public class CompanyController : BaseController
    {
        public CompanyController(IConfiguration configuration) : base(configuration)
        {
        }


        [HttpGet()]
        public async Task<IActionResult> List(string key = "", string location = "")
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Company, configuration)) return RedirectToAction("Index", "Home");

            var resultOfRequest = (await API.GetData<ResultDto<PaginationDto<ApplicantDto>>>(configuration["GlobalSettings:ApiUrl"],
                $"Applicant/GetApplicants?key={key}&location={location}", Request.Cookies["AuthToken"])).Data;
            ViewData["CurrentPage"] = 1;
            return View(resultOfRequest);
        }

        [HttpGet()]
        public async Task<IActionResult> AjaxSearch(string key = "", string location = "", int currentPage = 1)
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Company, configuration)) return RedirectToAction("Index", "Home");
            var resultOfRequest = (await API.
                GetData<ResultDto<PaginationDto<ApplicantDto>>>(configuration["GlobalSettings:ApiUrl"],
                $"Applicant/GetApplicants?currentPage={currentPage}&key={key}&location={location}", Request.Cookies["AuthToken"])).Data;

            ViewData["CurrentPage"] = currentPage;
            return PartialView("_AjaxSearch", resultOfRequest);
        }

        public async Task<IActionResult> Profile(string token=null)
        {
            if(!string.IsNullOrEmpty(token))
            {
                HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMonths(3) });
                if (!await CheckUserAccess.IsUserHasAccess(token, ClientType.Company, configuration)) return RedirectToAction("Index", "Home");
                var result = await API.GetData<ResultDto<CompanyDto>>(configuration["GlobalSettings:ApiUrl"],
                   "Company/GetCompanyByEmail", token);
                if (result != null && result.MessageCode == 0)
                {
                    return View(result.Data);
                }
            }
            else
            {
                if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Company, configuration)) return RedirectToAction("Index", "Home");


                var result = await API.GetData<ResultDto<CompanyDto>>(configuration["GlobalSettings:ApiUrl"],
                   "Company/GetCompanyByEmail", Request.Cookies["AuthToken"]);
                if (result != null && result.MessageCode == 0)
                {
                    return View(result.Data);
                }
            }

          
           
            return View();

        }

        public async Task<IActionResult> Offer()
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Company, configuration)) return RedirectToAction("Index", "Home");


            var CheckProfile = await API.GetData<CompanyDto>(configuration["GlobalSettings:ApiUrl"],
                "Company/GetCompany", Request.Cookies["AuthToken"]);

            if (string.IsNullOrWhiteSpace(CheckProfile.Phone))
            {
                await Profile();
                return View("Profile");
            }
            var result = await API.GetData<ResultDto<JobOfferViewModel>>(configuration["GlobalSettings:ApiUrl"],
                "Company/GetOfferViewModel", Request.Cookies["AuthToken"]);
            if (result.MessageCode == 0)
            {
                return View(result.Data);
            }
            return View();
        }
        [HttpPost()]
        public async Task<ActionResult> SaveOffer([FromForm] OfferDto offerDto)
        {
            var result = await API.PostData<ResultDto<int>>(configuration["GlobalSettings:ApiUrl"],
                "Company/SaveOrUpdateJobOffer", offerDto, Request.Cookies["AuthToken"]);
            if (result.MessageCode == 0)
            {
                return new JsonResult(new { success = "success", offerId = result.Data });
            }
            return new JsonResult(new { success = "Error", offerId = 0 });
        }
        [HttpPost()]
        public async Task<IActionResult> SaveCompany([FromForm] CompanyDto companyDto)
        {
            var data = await API.PostData<ResultDto<bool>>(configuration["GlobalSettings:ApiUrl"],
                "Company/UpdateCompany"
                , companyDto, Request.Cookies["AuthToken"]);
            if (data is { MessageCode: (int)MessageCodes.Success })
            {
                return data.Data ? new JsonResult(new { success = "success" }) : new JsonResult(new { success = "Error" });
            }
            return new JsonResult(new { success = "Error" });
        }

        [HttpGet()]
        public async Task<IActionResult> ApplicantDetails(int id)
        {
            if (!await CheckUserAccess.IsUserHasAccess(Request.Cookies["AuthToken"], ClientType.Company, configuration)) return RedirectToAction("Index", "Home");

            var result = await API.GetData<ResultDto<ApplicantDto>>(configuration["GlobalSettings:ApiUrl"], $"Applicant/GetApplicantById?id=" + id, null);
            return View(result.Data);

        }
    }
}
