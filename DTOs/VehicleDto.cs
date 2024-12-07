using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class VehicleDto
    {
        public string? UserName { get; set; }
        public string Marca { get; set; } 
        public int CantidadPuertas { get; set; } 
        public string Color { get; set; } 
        public string NumeroPlaca { get; set; } 
        public string TipoVehiculo { get; set; } 
    }
}
