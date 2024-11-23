using Microsoft.AspNetCore.Mvc;
using DTOs;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private static readonly List<Vehiculo> Vehiculos = new();

        /// <summary>
        /// Registra un nuevo vehículo.
        /// </summary>
        /// <param name="vehiculo">Información del vehículo</param>
        /// <returns>Vehículo registrado</returns>
        [HttpPost]
        public ActionResult<Vehiculo> RegistrarVehiculo([FromBody] Vehiculo vehiculo)
        {
            if (vehiculo == null)
                return BadRequest("La información del vehículo no puede ser nula.");

            Vehiculos.Add(vehiculo);
            return CreatedAtAction(nameof(ObtenerVehiculo), new { placa = vehiculo.NumeroPlaca }, vehiculo);
        }

        /// <summary>
        /// Obtiene un vehículo por su número de placa.
        /// </summary>
        /// <param name="placa">Número de placa</param>
        /// <returns>Información del vehículo</returns>
        [HttpGet("{placa}")]
        public ActionResult<Vehiculo> ObtenerVehiculo(string placa)
        {
            var vehiculo = Vehiculos.FirstOrDefault(v => v.NumeroPlaca == placa);
            if (vehiculo == null)
                return NotFound($"El vehículo con número de placa {placa} no fue encontrado.");

            return Ok(vehiculo);
        }
    }
}
