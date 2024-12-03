using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Marca { get; set; }
        public int CantidadPuertas { get; set; }
        public string Color { get; set; }

        [Required]
        public string NumeroPlaca { get; set; }
        public string TipoVehiculo { get; set; }

        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
   