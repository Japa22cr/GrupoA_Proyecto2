using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DisputeDto
    {
        public int FineId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
