using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.Models
{
    public class Dispute
    {
        public int Id { get; set; }

        [Required]
        public int FineId { get; set; }
        [ForeignKey("FineId")]
        public Fine Fine { get; set; }

        [Required]
        public string UserId { get; set; } // User who initiated the dispute
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string Reason { get; set; } // Reason for the dispute

        public string? Resolution { get; set; } // Admin-provided resolution, if any
        public DateTime? ResolutionDate { get; set; } // Date of resolution
        public bool IsResolved { get; set; } // Status of the dispute

        // Added property for judge user
        public string? JudgeId { get; set; } // Admin user who resolved the dispute
        [ForeignKey("JudgeId")]
        public ApplicationUser Judge { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Date the dispute was created
    }
}
