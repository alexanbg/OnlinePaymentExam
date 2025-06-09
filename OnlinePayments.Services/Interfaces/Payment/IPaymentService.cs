using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Models;
using OnlinePayments.Services.DTOs.Payments;

namespace OnlinePayments.Services.Interfaces.Payment
{
    public interface IPaymentService
    {
        public Task<PaymentInfo> MapToPaymentInfo(Models.Payment payment);
        public Task<CancelPaymentResponse> CancelPayment(CancelPaymentRequest request);
        public Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);
        public Task<SendPaymentResponse> SendPayment(SendPaymentRequest request);
        public Task<ShowPaymentsResponse> ShowPayments(ShowPaymentsRequest request);
    }
}
