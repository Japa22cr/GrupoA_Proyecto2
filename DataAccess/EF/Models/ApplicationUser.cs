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

        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Fine> Fines { get; set; }

        public ApplicationUser() 
        {
            Vehicles = new List<Vehicle>();
            Fines = new List<Fine>();
        }
    }   
}
