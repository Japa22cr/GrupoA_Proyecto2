using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PaymentRequestDto
    {
        public string PaymentId { get; set; }         // Unique identifier for the payment request
        public decimal Amount { get; set; }          // Amount to be paid
        public string Currency { get; set; }         // Currency code (e.g., "USD", "EUR")
        public string CustomerId { get; set; }       // Unique identifier for the customer
        public string PaymentMethod { get; set; }    // Payment method (e.g., "CreditCard", "PayPal")
        public string Description { get; set; }      // Description of the transaction
        public DateTime PaymentDate { get; set; }    // Date and time of the payment request
    }

}
