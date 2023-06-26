using FindJobs.Domain.Dtos.Email;
using FindJobs.Domain.Enums;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IEmailService
    {
        Task<EmailSendResult> SendPaymentEmail(SendPaymentEmail payment);

    }
}
