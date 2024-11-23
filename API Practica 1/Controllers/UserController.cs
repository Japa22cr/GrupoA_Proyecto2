using BL.IServices;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using DataAccess.EF.Models;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Practica_1.Controllers
{

        [ApiController]
        [Route("api/[controller]/[action]")]
        public class UserController : ControllerBase
        {

            private readonly UserManager<ApplicationUser> _userManager;
            public UserController(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }


            [HttpGet]
            public async Task<IActionResult> GetAllUsers()
            {
                try
                {
                    // Fetch all users
                    var users = await _userManager.Users.ToListAsync();

                    // Filter users excluding those with the "Admin" role
                    var filteredUsers = new List<object>();

                    foreach (var user in users)
                    {
                        var roles = await _userManager.GetRolesAsync(user); // Get roles of the user

                        if (!roles.Contains("Admin"))
                        {
                            filteredUsers.Add(new
                            {

                                user.UserName,
                                user.Email,
                                user.FirstName, // Accessing custom properties
                                user.LastName,
                                Role = roles


                            });
                        }
                    }

                    // Check if there are any users after filtering
                    if (filteredUsers.Count == 0)
                    {
                        return NotFound("No users found.");
                    }

                    return Ok(filteredUsers);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

    }
}
