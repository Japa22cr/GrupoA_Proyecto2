using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class FotosDto
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Nacionalidad { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public IFormFile FotoPerfil { get; set; }

        [Required]
        public IFormFile FotoCedula { get; set; }

        public string FotoPerfilBase64 { get; set; }
        public string FotoCedulaBase64 { get; set; }
    }
}
