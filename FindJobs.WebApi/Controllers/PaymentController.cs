using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Dtos.Email;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Services;
using FindJobs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindJobs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly IEmailService emailService;
        public PaymentController(IPaymentService paymentService, IEmailService emailService)
        {
            this.paymentService = paymentService;
            this.emailService = emailService;
        }

        [HttpPost("CreatePayment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreatePayment(PaymentDto payment)
        {
            if (!paymentService.CreatePayment(payment))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            return new JsonResult(new ResultDto((int)MessageCodes.Success));
        }

        [HttpPost("UpdatePaymentDetails")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdatePaymentDetails(PaymentDto payment)
        {
            if (!paymentService.UpdatePayment(payment))
                return new JsonResult(new ResultDto((int)MessageCodes.BadRequest));
            return new JsonResult(new ResultDto((int)MessageCodes.Success));
        }

        [HttpPost("GetPayment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetPayment(string au)
        {
            return new JsonResult(new ResultDto<PaymentDto>(paymentService.GetPayment(au)));
        }

        [HttpPost("SendPaymentLink")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendPaymentLink(SendPaymentEmail payment)
        {
            if(string.IsNullOrWhiteSpace(payment.Email))
            {
                var claims = User.Claims.ToList();
                var email = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
                payment.Email = email;
            }
            var res = await emailService.SendPaymentEmail(payment);
            return res switch
            {
                EmailSendResult.Success => new JsonResult((new ResultDto((int)MessageCodes.Success))),
                _ => new JsonResult(new ResultDto((int)MessageCodes.BadRequest))
            };

        }
    }
}
