using FindJobs.Domain.Dtos;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IMailSenderService
    {
        Task<bool> sendMail(MailModelDto mailModel,string From);
    }
}
