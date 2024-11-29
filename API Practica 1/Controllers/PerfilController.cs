using DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FotosController : ControllerBase
    {
        private static List<FotosDto> perfiles = new List<FotosDto>();
        private readonly string _imagenesPath = Path.Combine(Directory.GetCurrentDirectory(), "imagenes");

        public FotosController()
        {
            if (!Directory.Exists(_imagenesPath))
            {
                Directory.CreateDirectory(_imagenesPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearPerfil([FromForm] FotosDto fotosDto)
        {
            try
            {
                // Guardar la foto de perfil
                if (fotosDto.FotoPerfil != null)
                {
                    var fotoPerfilPath = Path.Combine(_imagenesPath, fotosDto.FotoPerfil.FileName);
                    using (var stream = new FileStream(fotoPerfilPath, FileMode.Create))
                    {
                        await fotosDto.FotoPerfil.CopyToAsync(stream);
                    }

                    // Convertir a Base64
                    fotosDto.FotoPerfilBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(fotoPerfilPath));
                }

                // Guardar la foto de cédula
                if (fotosDto.FotoCedula != null)
                {
                    var fotoCedulaPath = Path.Combine(_imagenesPath, fotosDto.FotoCedula.FileName);
                    using (var stream = new FileStream(fotoCedulaPath, FileMode.Create))
                    {
                        await fotosDto.FotoCedula.CopyToAsync(stream);
                    }

                    // Convertir a Base64
                    fotosDto.FotoCedulaBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(fotoCedulaPath));
                }

                perfiles.Add(fotosDto);

                return CreatedAtAction(nameof(ObtenerPerfil), new { cedula = fotosDto.Cedula }, fotosDto);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al crear perfil: " + ex.Message);
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        [HttpGet("{cedula}")]
        public IActionResult ObtenerPerfil(string cedula)
        {
            var perfil = perfiles.FirstOrDefault(p => p.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase));
            if (perfil == null)
            {
                return NotFound("Perfil no encontrado.");
            }

            // Convertir las imágenes almacenadas en disco a Base64 si no están ya en memoria
            if (string.IsNullOrEmpty(perfil.FotoPerfilBase64) && perfil.FotoPerfil != null)
            {
                var fotoPerfilPath = Path.Combine(_imagenesPath, perfil.FotoPerfil.FileName);
                perfil.FotoPerfilBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(fotoPerfilPath));
            }

            if (string.IsNullOrEmpty(perfil.FotoCedulaBase64) && perfil.FotoCedula != null)
            {
                var fotoCedulaPath = Path.Combine(_imagenesPath, perfil.FotoCedula.FileName);
                perfil.FotoCedulaBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(fotoCedulaPath));
            }

            return Ok(perfil);
        }

    }
}
