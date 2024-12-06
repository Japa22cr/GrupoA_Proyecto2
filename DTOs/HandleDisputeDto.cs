using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class HandleDisputeDto
    {
        public int DisputeId { get; set; }
        public string Resolution { get; set; }
        public string JudgeId { get; set; }
    }
}
