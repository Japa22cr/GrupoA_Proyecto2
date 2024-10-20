﻿using System.ComponentModel.DataAnnotations;

namespace DataAccess.EF.Models
{
    public class ApplicationUser 
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
