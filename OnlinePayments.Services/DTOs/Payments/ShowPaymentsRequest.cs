using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class ShowPaymentsRequest
    {
        public int UserId { get; set; }
        public bool SortByStatus { get; set; }
    }
}
