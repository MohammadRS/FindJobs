using FindJobs.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNek.Common.Models;
using DotNek.Common.Helpers;
using FindJobs.Domain.Enums;
using FindJobs.Web.Models;

namespace FindJobs.Web.Controllers
{
    public class AuthController : BaseController
    {
        HttpClient client;

        public AuthController(IConfiguration configuration) : base(configuration)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(configuration["GlobalSettings:ApiUrl"]);
        }

        [HttpPost()]
        public async Task<IActionResult> SendEmailVerifyCode([FromBody] VerifyCodeViewModel sendEmail)
        {
            var baseUrl = configuration["GlobalSettings:ApiUrl"];
            var response = await API.GetData<ResultDto>(baseUrl, "auth/SendVerificationCode?email=" + sendEmail.Email + "&captcha=" + sendEmail.Captcha + "&type=" + sendEmail.Type + "&platformType=" + PlatformType.Web);
            if (response.MessageCode == 0)
            {
                return new JsonResult(new ResultDto((int)MessageCodes.Success));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.CaptchaNotValid));
        }

        [HttpPost()]
        public async Task<IActionResult> LoginWithEmail([FromBody] VerifyCodeViewModel sendEmail)
        {
            var baseUrl = configuration["GlobalSettings:ApiUrl"];
            var response = await API.GetData<ResultDto<TokenResDto>>(baseUrl, "auth/SignInWithVerificationCode?email=" + sendEmail.Email + "&verificationCode=" + sendEmail.Code + "&type=" + sendEmail.Type);
            if (response.MessageCode == (int)MessageCodes.Success)
            {
                HttpContext.Response.Cookies.Append("AuthToken", response.Data.Token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMonths(3) });
                return new JsonResult(new ResultDto<TokenResDto>(response.Data));
            }
            else if (response.Data.MessageCode == MessageCodes.VerificationCodeNotValid)
            {
                return new JsonResult(new ResultDto((int)MessageCodes.VerificationCodeNotValid));
            }


            return new JsonResult(new ResultDto((int)MessageCodes.CaptchaNotValid));
        }
        [HttpGet()]
        public async Task<IActionResult> LoginWithGoogle(string token)
        {
            var baseUrl = configuration["GlobalSettings:ApiUrl"];
            var result = await API.GetData<ResultDto<TokenResDto>>(baseUrl, "Auth/SignInWithGoogle?token=" + token);
            if (result != null)
            {
                if (result.MessageCode == (int)MessageCodes.Success)
                {
                    HttpContext.Response.Cookies.Append("AuthToken", result.Data.Token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMonths(3) });
                    return new JsonResult(new ResultDto<TokenResDto>(result.Data));
                }
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }
        [HttpGet]
        [ActionName("LoginWithFacebook")]
        public async Task<IActionResult> LoginWithFacebook(string FacebookToken)
        {
                var baseUrl = configuration["GlobalSettings:ApiUrl"];
                var result = await API.PostData<ResultDto<TokenResDto>>(baseUrl, "Auth/SignInWithFacebook", FacebookToken);
                if (result != null)
                {
                    if (result.MessageCode == (int)MessageCodes.Success)
                    {
                        HttpContext.Response.Cookies.Append("AuthToken", result.Data.Token, new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddMonths(3) });
                        return new JsonResult(new ResultDto<TokenResDto>(result.Data));
                    }
                    return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            }

            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }
       
        public IActionResult LogOut(string culture)
        {
          
            HttpContext.Response.Cookies.Delete("AuthToken");
            return Redirect("/"+culture+"/");
        }


    }
}



