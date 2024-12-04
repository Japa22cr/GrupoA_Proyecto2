using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class FineDto
    {

        public string Inspector { get; set; }

        public string UserEmail { get; set; }

        public string Place { get; set; }

        public string LicensePlate { get; set; }

        public string Category { get; set; }

        public string Article { get; set; }

        public string Description { get; set; }

        public string Conduct { get; set; }

        public decimal Amount { get; set; }

        public bool Estado { get; set; }
    }
}