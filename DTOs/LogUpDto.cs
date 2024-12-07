using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class LogUpDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdDocument { get; set; }
        public IFormFile ProfilePicture { get; set; } // Foto de perfil
        public IFormFile DocumentPicture { get; set; } // Foto de cédula
    }
}
