using AutoMapper;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using System.Linq;

namespace FindJobs.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> paymentRepository;
        private readonly IMapper mapper;

        public PaymentService(IMapper mapper, IGenericRepository<Payment> paymentRepository)
        {
            this.mapper = mapper;
            this.paymentRepository = paymentRepository;
        }

        public bool CreatePayment(PaymentDto command)
        {
            if (paymentRepository.GetEntities().Any(x => x.OfferId == command.OfferId))
                return false;
            var payment = mapper.Map<Payment>(command);
            paymentRepository.AddEntity(payment);
            paymentRepository.SaveChange();
            return true;

        }

        public PaymentDto GetPayment( string au)
        {
            return mapper.Map<PaymentDto>(paymentRepository.
                GetEntities().FirstOrDefault(x =>x.Authority == au));
        }

        public bool UpdatePayment(PaymentDto command)
        {
            var payment = paymentRepository.GetEntities().FirstOrDefault(x => x.OfferId == command.OfferId);
            if (payment is null)
                return false;
            payment.UpdatePayment(command.Authority, command.Amount
                , command.GetwayType, command.Currency, command.Done, command.TransactionId, command.IsRefunded);
            paymentRepository.SaveChange();
            return true;

        }
    }
}
