using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Services;
using FindJobs.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DotNek.Common.Models;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IConfiguration configuration;
   

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            this.authService = authService;
            this.configuration = configuration;
        }
        [HttpGet("SendVerificationCode")]
        public async Task<IActionResult> SendVerificationCode(string email, string captcha, int type, PlatformType platformType)
        {
            var GoogleRechaptchaTools = new AuthServerSideValidation();
            var secretKey = platformType switch
            {
                PlatformType.Web => configuration["GlobalSettings:GoogleCaptchaSecretKey"],
                PlatformType.Android => configuration["GlobalSettings:GoogleCaptchaAndroidSecretKey"],
                PlatformType.iOS => configuration["GlobalSettings:GoogleCaptchaSecretKey"],
                PlatformType.UWP => configuration["GlobalSettings:GoogleCaptchaSecretKey"],
                _ => string.Empty
            };
            var capResponse = GoogleRechaptchaTools.CheckRecaptcha(captcha, secretKey);
            if (capResponse.Success)
            {
                if (string.IsNullOrWhiteSpace(email)) return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
                var res = await authService.SendVerificationCode(email);


                return res switch
                {
                    EmailSendResult.Success => new JsonResult(new { data = "Ok" }),
                    _ => new JsonResult(new ResultDto((int)MessageCodes.BadRequest))
                };
            }


            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

        }
        [HttpGet("SignInWithVerificationCode")]
        public async Task<IActionResult> SignInWithVerificationCode(string verificationCode, string email, int type)
        {
            if (string.IsNullOrWhiteSpace(verificationCode) || string.IsNullOrWhiteSpace(email))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));

            var tokenResDto = await authService.SignInWithVerificationCode(email, verificationCode.Trim(), type);
            if (tokenResDto.MessageCode == MessageCodes.Success)
                return new JsonResult(new ResultDto<TokenResDto>(tokenResDto));
            if (tokenResDto.MessageCode == MessageCodes.VerificationCodeNotValid)
                return new JsonResult(new ResultDto<TokenResDto>(new TokenResDto { Token = "", MessageCode = MessageCodes.VerificationCodeNotValid }));

            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }

        [HttpGet("SignInWithGoogle")]
        public async Task<IActionResult> SignInWithGoogle(string token)
        {

            var tokenResDto = await authService.SignInWithGoogle(token);
            if (tokenResDto.MessageCode == MessageCodes.Success)
            {
                return new JsonResult(new ResultDto<TokenResDto>(tokenResDto));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }

        [HttpPost("SignInWithFacebook")]
        public async Task<IActionResult> SignInWithFacebook([FromBody] string FacebookToken)
        {
            var serverSideValidation = new AuthServerSideValidation();
            var result = await serverSideValidation.ValidateFaceBookToken(FacebookToken);
            if (result.email != null)
            {
                var tokenResDto = await authService.SignInWithFacebook(result);
                if (tokenResDto != null)
                {
                    if (tokenResDto.MessageCode == MessageCodes.Success)
                    {
                        return new JsonResult(new ResultDto<TokenResDto>(tokenResDto));
                    }
                }
            }

            else
            {
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            }
            return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
        }

    }
}

