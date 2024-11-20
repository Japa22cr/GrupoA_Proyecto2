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
            return CreatedAtAction(nameof(ObtenerVehiculo), new { vin = vehiculo.VIN }, vehiculo);
        }

        /// <summary>
        /// Obtiene un vehículo por su VIN.
        /// </summary>
        /// <param name="vin">Número de VIN</param>
        /// <returns>Información del vehículo</returns>
        [HttpGet("{vin}")]
        public ActionResult<Vehiculo> ObtenerVehiculo(string vin)
        {
            var vehiculo = Vehiculos.FirstOrDefault(v => v.VIN == vin);
            if (vehiculo == null)
                return NotFound($"El vehículo con VIN {vin} no fue encontrado.");

            return Ok(vehiculo);
        }
    }
}
