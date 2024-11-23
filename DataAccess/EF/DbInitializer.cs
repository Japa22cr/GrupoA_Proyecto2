using DataAccess.EF.Models;
using DataAccess.EF.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF
{
    public class DbInitializer
    {
        public static async Task Initialize(ClaseDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            // Crear la base de datos si no existe
            context.Database.EnsureCreated();

            // Crear roles si no existen
            string[] roleNames = { "User", "Judge", "Officer", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Crear un usuario de ejemplo y asignar rol
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "lenin.ugalde21@gmail.com"
            };

            string adminPassword = "Password123!";
            var adminResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}
