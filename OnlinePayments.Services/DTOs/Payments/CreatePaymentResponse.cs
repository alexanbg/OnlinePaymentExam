using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class CreatePaymentResponse
    {
        public bool Success { get; set; }
        public PaymentInfo? CreatedPayment {  get; set; }
        public string? Error { get; set; }
    }
}
