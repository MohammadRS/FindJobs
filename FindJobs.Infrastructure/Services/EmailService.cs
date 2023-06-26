using FindJobs.Domain.Dtos;
using FindJobs.Domain.Dtos.Email;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRazorPartialToStringService renderer;
        private readonly IMailSenderService sender;
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration, IRazorPartialToStringService renderer, IMailSenderService sender)
        {
            this.configuration = configuration;
            this.renderer = renderer;
            this.sender = sender;
        }


        public async Task<EmailSendResult> SendPaymentEmail(SendPaymentEmail payment)
        {
            var mailModel = new MailModelDto();
            mailModel.Email = payment.Email;
            var model = new SendPaymentEmail
            {
                HeaderTitle = Res.Email.HeaderTitle,
                HeaderSubTitle = Res.Email.HeaderSubTitle,
                InstagramLink = configuration["GlobalSettings:InstagramLink"],
                FacebookLink = configuration["GlobalSettings:FacebookLink"],
                TwitterLink = configuration["GlobalSettings:TwitterLink"],
                Email = payment.Email,
                PayableAmount = payment.Price + payment.VAT,
                PayPalPaymentLink = payment.PayPalPaymentLink,
                CrditCardPaymentLink = "https://www.bank.com/",//TODO Fake Data (ask hossein)
                Price = payment.Price,
                Product = payment.Product,
                VAT = payment.VAT,
                CurrencyType=payment.CurrencyType
               
            };
            var body = await renderer.RenderPartialToStringAsync("/Views/Email/Payment.cshtml", model);
            mailModel.Body = body;
            mailModel.Subject = $"Order #{payment.OfferId}";
            mailModel.to = payment.Email;
            if (await sender.sendMail(mailModel, "DotNek Team"))
            {
                return EmailSendResult.Success;

            }
            return EmailSendResult.NotSend;
        }
    }
}
