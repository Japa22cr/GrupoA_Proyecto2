using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PaymentResponseDto
    {
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
