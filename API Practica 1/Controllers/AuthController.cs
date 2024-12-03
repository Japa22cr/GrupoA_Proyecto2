using BL.IServices;
using DataAccess.EF.Models;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;            
            _emailService = emailService;
            _roleManager = roleManager;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto userData)
        {
            var user = await _userManager.FindByNameAsync(userData.UserName) as ApplicationUser;
            if (user != null && await _userManager.CheckPasswordAsync(user, userData.Password))
            {
                // Generate a secure 2FA code
                var code = new Random().Next(1000, 9999).ToString();

                // Store the code in the user record with an expiry time
                user.TwoFactorCode = code;
                user.TwoFactorExpiry = DateTime.UtcNow.AddMinutes(5); // Code valid for 5 minutes

                await _userManager.UpdateAsync(user);

                // Send the code via email
                await _emailService.SendEmailAsync(user.Email, "Autenticacion de Doble Factor", $"Su Codigo de seguridad es: {code}");

                return Ok(new { message = "2FA code sent to your email." });
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] TwoFactorDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName) as ApplicationUser;
            if (user != null)
            {
                // Check if the code matches and is not expired
                if (user.TwoFactorCode == model.Code && user.TwoFactorExpiry > DateTime.UtcNow)
                {
                    // Clear the 2FA code after verification
                    user.TwoFactorCode = null;
                    user.TwoFactorExpiry = null;
                    await _userManager.UpdateAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = roles.FirstOrDefault() ?? "User";

                    // Generate JWT token
                    var jwToken = await GenerateJwtToken(user);
                    return Ok(new { username = user.UserName, role = userRole, token = jwToken });
                }
            }
            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);  // Retrieve the roles for the user
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LogUpDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                IdDocument = newUser.IdDocument
            };

            var createdUserResult = await _userManager.CreateAsync(user, newUser.Password);

            if (createdUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Created("Usuario creado exitosamente", null);
            }

            foreach (var error in createdUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> InternalRegister([FromBody] InternalRegisterDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate role
            var validRoles = new[] { "User", "Judge", "Officer", "Admin" };
            if (!validRoles.Contains(newUser.Role))
            {
                return BadRequest("Invalid role specified");
            }

            // Create a new user
            var user = new ApplicationUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                IdDocument = newUser.IdDocument
            };

            var createdUserResult = await _userManager.CreateAsync(user, newUser.Password);

            if (!createdUserResult.Succeeded)
            {
                foreach (var error in createdUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Check if role exists before assigning
            var roleExists = await _roleManager.RoleExistsAsync(newUser.Role); // Requires RoleManager<T> injection
            if (!roleExists)
            {
                // Cleanup: Delete the user since the role assignment is invalid
                await _userManager.DeleteAsync(user);
                return BadRequest($"The role '{newUser.Role}' does not exist.");
            }

            // Assign the role
            var roleResult = await _userManager.AddToRoleAsync(user, newUser.Role);
            if (!roleResult.Succeeded)
            {
                // Cleanup: Delete the user since role assignment failed
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Return success response
            return CreatedAtAction(nameof(InternalRegister), new { id = user.Id }, new
            {
                message = "Usuario creado exitosamente",
                userId = user.Id,
                userName = user.UserName,
                role = newUser.Role
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null || !roles.Any())
            {
                return Ok("User has no assigned roles");
            }

            return Ok(roles); // Return the roles as a list of strings
        }

        [HttpGet]
        public async Task<IActionResult> MakeAdmin(string userName)
        {
            // Buscar al usuario por su nombre de usuario
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Agregar el rol 'Admin' al usuario
            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return Ok("Usuario agregado al rol de Admin con éxito");
            }

            // Si hubo algún error al agregar el rol
            return BadRequest("No se pudo agregar el rol de Admin al usuario");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok("");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{model.ResetUrl}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            await _emailService.SendResetPasswordEmail(user.Email, resetUrl);

            return Ok("Email Sent succesfully");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok("Password has been reset successfully. ");
            }
            return BadRequest("Failed to reset password.");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateResetToken([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new { Token = token });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Fetch all users' IDs (efficient way to get list of user IDs without fetching full user data)
                var userIds = await _userManager.Users.Select(u => u.Id).ToListAsync();

                var filteredUsers = new List<object>();

                foreach (var userId in userIds)
                {
                    // Fetch the user by ID
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user); // Get roles of the user

                        if (!roles.Contains("Admin"))
                        {
                            filteredUsers.Add(new
                            {
                                user.UserName,
                                user.Email,
                                //user.FirstName, // Accessing custom properties
                                //user.LastName,
                                Role = roles
                            });
                        }
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
