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
            // Validar que el modelo no sea nulo
            if (model == null)
            {
                return BadRequest("Datos inválidos en la solicitud.");
            }
            string userId = null;
            // Si se proporciona UserName, buscar el usuario
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return BadRequest("Usuario no encontrado.");
                }
                userId = user.Id; // Asignar el ID del usuario si se encuentra
            }
            // Crear el vehículo
            var vehicle = new Vehicle
            {
                UserName = model.UserName, // Esto puede ser nulo si no se proporcionó
                UserId = userId,          // También puede ser nulo
                NumeroPlaca = model.NumeroPlaca,
                CantidadPuertas = model.CantidadPuertas,
                Color = model.Color,
                TipoVehiculo = model.TipoVehiculo,
                Marca = model.Marca
            };
            try
            {
                // Agregar el vehículo a la base de datos
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return Ok("Vehículo agregado con éxito.");
            }
            catch (Exception ex)
            {
                // Manejar errores durante la operación
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
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
                        v.CantidadPuertas,
                        v.Color,
                        v.NumeroPlaca,
                        v.TipoVehiculo,
                        v.Marca
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

        [HttpGet("by-plate/{plateNumber}")]
        public async Task<IActionResult> GetVehicleByPlate(string plateNumber)
        {
            // Validar que el número de placa no sea nulo o vacío
            if (string.IsNullOrEmpty(plateNumber))
            {
                return BadRequest("El número de placa es requerido.");
            }
            try
            {
                // Buscar el vehículo por número de placa
                var vehicle = await _context.Vehicles
                    .Where(v => v.NumeroPlaca == plateNumber)
                    .Select(v => new VehicleDto
                    {
                        UserName = v.UserName,
                        Marca = v.Marca,
                        CantidadPuertas = v.CantidadPuertas,
                        Color = v.Color,
                        NumeroPlaca = v.NumeroPlaca,
                        TipoVehiculo = v.TipoVehiculo
                    })
                    .FirstOrDefaultAsync();
                // Verificar si el vehículo existe
                if (vehicle == null)
                {
                    return NotFound("No se encontró ningún vehículo con ese número de placa.");
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                // Manejar errores durante la consulta
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
