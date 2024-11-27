using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class FineDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string UserEmail { get; set; }
    }
}
