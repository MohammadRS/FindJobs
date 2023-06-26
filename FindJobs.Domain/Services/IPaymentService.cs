using FindJobs.Domain.Dtos;

namespace FindJobs.Domain.Services
{
    public interface IPaymentService
    {
        bool CreatePayment(PaymentDto command);
        bool UpdatePayment(PaymentDto command);
        PaymentDto GetPayment(string au);    
    
    }
}
