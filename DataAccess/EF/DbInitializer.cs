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
            // Define base users
            var baseUsers = new List<(string UserName, string Email, string Password, string Role)>
            {
                ("admin", "lenin.ugalde21@gmail.com", "Password123!", "Admin"),
                ("moka", "mokacris22@gmail.com", "Password123!", "Judge"),
                ("kroxz", "kylekroxz@gmail.com", "Password123!", "Officer"),
                ("ugalde", "ugaldelenin500@gmail.com", "Password123!", "User")
            };

            // Create each user
            foreach (var (userName, email, password, role) in baseUsers)
            {
                // Check if user already exists
                var existingUser = await userManager.FindByNameAsync(userName);
                if (existingUser == null)
                {
                    // Create new user
                    var newUser = new ApplicationUser
                    {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true // Optionally set email as confirmed
                    };

                    var createResult = await userManager.CreateAsync(newUser, password);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, role);
                    }
                    else
                    {
                        // Log any errors during user creation
                        Console.WriteLine($"Failed to create user {userName}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"User {userName} already exists.");
                }
            }
        }

    }
}
