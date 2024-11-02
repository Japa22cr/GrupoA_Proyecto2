using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string ResetUrl { get; set; }
    }
}
