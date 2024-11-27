using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.Models
{
    public class Fine
    {
        public int Id { get; set; }

        public string Inspector { get; set; }

        public DateTime IssuedDate { get; set; }

        public string Place { get; set; }

        public string LicensePlate { get; set; }

        public string Category { get; set; }

        public string Article { get; set; }

        public string Description { get; set; }

        public string Conduct { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        // One-to-One relationships
        public int? DisputeId { get; set; }
        public Dispute Dispute { get; set; }

        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }

        // Property to track if no further action is allowed
        public bool IsFinalized => Dispute != null || Payment != null;
    }
}
