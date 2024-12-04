using Microsoft.AspNetCore.Mvc;
using DTOs;
using DataAccess.EF.Models;
using DataAccess.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly ClaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VehiclesController(ClaseDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto model)
        {
            // Validate the request body
            if (model == null || string.IsNullOrEmpty(model.UserName))
            {
                return BadRequest("Invalid request data.");
            }

            // Find the user by email
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Create a new Fine record
            var vehicle = new Vehicle
            {
                UserName = model.UserName,
                UserId = user.Id,
                Marca = model.Marca,
                NumeroPlaca = model.NumeroPlaca,
                CantidadPuertas = model.CantidadPuertas,
                Color = model.Color,
                TipoVehiculo = model.TipoVehiculo,
            };

            try
            {
                // Add the Fine to the database
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                return Ok("Vehicle added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during database save
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GerVehiclesByUser(string userin)
        {
            // Validar el parámetro de correo electrónico
            if (string.IsNullOrEmpty(userin))
            {
                return BadRequest("El usuario es requerido.");
            }

            // Buscar al usuario por correo electrónico
            var user = await _userManager.FindByNameAsync(userin);
            if (user == null)
            {
                return NotFound("No se ha encontrado un usuario.");
            }

            try
            {
                // Obtener todas las multas asociadas al usuario
                var vehicles = await _context.Vehicles
                    .Where(v => v.UserId == user.Id)
                    .Select(v => new
                    {
                        v.Id,
                        v.Marca,
                        v.CantidadPuertas,
                        v.Color,
                        v.NumeroPlaca,
                        v.TipoVehiculo

                    })
                    .ToListAsync();

                // Verificar si el usuario tiene multas
                if (!vehicles.Any())
                {
                    return NotFound("No se encuentra ninguna multa.");
                }

                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                // Manejar errores que puedan ocurrir durante la consulta
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
