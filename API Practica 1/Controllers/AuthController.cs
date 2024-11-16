using BL.IServices;
using DataAccess.EF.Models;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

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

        private async Task<string> GenerateJwtToken(IdentityUser user)
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

        [HttpGet]
        public async Task<bool> RoleTesting(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                // El usuario no existe
                return false;
            }

            var result = await _userManager.IsInRoleAsync(user, "Admin");
            return result;
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

    }

}
