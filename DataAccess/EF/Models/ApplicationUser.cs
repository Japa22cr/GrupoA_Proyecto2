using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.EF.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string IdDocument { get; set; }
        public byte[] ProfilePicture { get; set; } // Foto de perfil en formato binario
        public byte[] DocumentPicture { get; set; } // Foto de cédula en formato binario
        public string TwoFactorCode { get; set; }
        public DateTime? TwoFactorExpiry { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Fine> Fines { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Dispute> Disputes { get; set; }
        public ApplicationUser() 
        {
            Vehicles = new List<Vehicle>();
            Fines = new List<Fine>();
            Payments = new List<Payment>();
            Disputes = new List<Dispute>();
        }
    }   
}
