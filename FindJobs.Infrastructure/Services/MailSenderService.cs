using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IRazorPartialToStringService renderer;

        public MailSenderService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
            IRazorPartialToStringService renderer)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.renderer = renderer;
        }
        public async Task<bool> sendMail(MailModelDto mailModel, string From)
        {
            var defaultEmail = configuration["MailSettings:EmailUsername"];

            var mail = new MailMessage();


            var SmtpServer = new SmtpClient(configuration["MailSettings:SmtpServer"]);

            mail.From = new MailAddress(defaultEmail, From);

            mail.To.Add(mailModel.to);

            mail.Subject = mailModel.Subject;

            mail.Body = mailModel.Body;
            mail = EmbedImages(mailModel.Body, mail);

            mail.IsBodyHtml = true;

            // System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = Convert.ToInt32(configuration["MailSettings:SmtpPort"]);

            SmtpServer.Credentials = new System.Net.NetworkCredential(defaultEmail, configuration["MailSettings:EmailPassword"]);

            SmtpServer.EnableSsl = true;

            SmtpServer.Timeout = 20000;
            try
            {
                await SmtpServer.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }



        public MailMessage EmbedImages(string Body, MailMessage mailMessage)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            string[] DesignAttachments = new[] { "logo.png", "facebook.png", "twitter.png", "instagram.png" };
            string webRootPath = webHostEnvironment.WebRootPath + "/images/";
            string folder = webRootPath;
            for (int i = 0; i <= DesignAttachments.Length - 1; i++)
            {
                LinkedResource ThemeAttach = new LinkedResource(Path.Combine(webRootPath + DesignAttachments[i]), "image/png");
                ThemeAttach.ContentId = System.IO.Path.GetFileName(DesignAttachments[i]);
                htmlView.LinkedResources.Add(ThemeAttach);
            }
            mailMessage.AlternateViews.Add(htmlView);
            return mailMessage;

        }

    }

}

