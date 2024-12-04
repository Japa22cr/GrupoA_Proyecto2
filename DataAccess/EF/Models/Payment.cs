using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int FineId { get; set; } // Foreign key to Fine
        [ForeignKey("FineId")]        public Fine Fine { get; set; }

        [Required]
        public string UserId { get; set; } // User who made the payment
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Amount paid

        [Required]
        public string PaymentMethod { get; set; } // e.g., Credit Card, PayPal
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // Date of payment
    }
}
