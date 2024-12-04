using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PaymentRequestDto
    {    
        public int FineId { get; set; }       
        public string PaymentMethod { get; set; }    
        public decimal Amount { get; set; }  
        public DateTime PaymentDate { get; set; }    
    }

}
