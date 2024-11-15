using DataAccess.EF.Models;
using DataAccess.EF;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinesController : ControllerBase
    {
        private readonly ClaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FinesController(ClaseDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddFine([FromBody] FineDto model)
        {
            // Validate the request body
            if (model == null || string.IsNullOrEmpty(model.UserEmail))
            {
                return BadRequest("Invalid request data.");
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Create a new Fine record
            var fine = new Fine
            {
                Amount = model.Amount,
                Description = model.Description,
                IssuedDate = DateTime.UtcNow,
                UserId = user.Id // Associate fine with the found user's Id
            };

            try
            {
                // Add the Fine to the database
                _context.Fines.Add(fine);
                await _context.SaveChangesAsync();

                return Ok("Fine added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during database save
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
