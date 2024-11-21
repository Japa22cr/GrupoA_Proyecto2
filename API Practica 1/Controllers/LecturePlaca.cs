using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Tesseract;

namespace API_Practica_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturaPlaca : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Validar formato y tamaño del archivo
            if (file == null || (file.ContentType != "image/png" && file.ContentType != "image/jpeg"))
                return BadRequest("El archivo debe ser una imagen PNG o JPEG.");

            if (file.Length > 5 * 1024 * 1024) // 5 MB límite
                return BadRequest("El archivo es demasiado grande.");

            try
            {
                string resultText;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    resultText = ConvertImageToText(imageBytes);
                }
                return Ok(resultText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando la imagen: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al procesar la imagen.");
            }
        }


        private static string ConvertImageToText(byte[] arrayImage)
        {
            //Logica de Tesseract
            var engine = new TesseractEngine("tessdata", "spa", EngineMode.Default);
            var image = Pix.LoadFromMemory(arrayImage);
            var page = engine.Process(image);

            string text = page.GetText();
            return text;
        }


    }
}