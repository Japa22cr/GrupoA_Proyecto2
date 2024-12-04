using BL.IServices;
using DataAccess.EF;
using DataAccess.EF.Models;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisputesController: ControllerBase
    {
        private readonly ClaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public DisputesController(ClaseDbContext context, UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // Dispute Fine

        [HttpPost]
        public async Task<IActionResult> DisputeFine([FromBody] DisputeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var fine = await _context.Fines.Include(f => f.Payment).FirstOrDefaultAsync(f => f.Id.Equals(model.FineId));

                if (fine == null)
                {
                    return NotFound("Fine not found.");
                }

                if (fine.Estado)
                {
                    return BadRequest("This fine is already resolved.");
                }

                var user = await _userManager.FindByIdAsync(fine.UserId);

                var dispute = new Dispute
                {
                    FineId = fine.Id,
                    UserId = user.Id,
                    Reason = model.Reason,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Disputes.Add(dispute);

                fine.DisputeId = dispute.Id;

                fine.Estado = true;

                await _context.SaveChangesAsync();

                // Send confirmation email
                //await _emailService.SendConfirmationEmailAsync(user.UserEmail, fine, payment);

                return Ok(new { Message = "Fine paid successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the payment.");
            }
        }

        // Handle Dispute

        // Get All Disputes

        [HttpGet]
        public async Task<IActionResult> GetAllDisputes()
        {
            try
            {
                // Obtener todas las multas asociadas al usuario
                var disputes = await _context.Disputes
                    .Select(d => new
                    {
                        d.Id,
                        d.Fine,
                        d.CreatedDate,
                        d.Reason,
                        d.IsResolved,
                        d.User
                    })
                    .ToListAsync();

                // Verificar si el usuario tiene multas
                if (!disputes.Any())
                {
                    return NotFound("No se encuentraron ninguna disputa.");
                }

                return Ok(disputes);
            }
            catch (Exception ex)
            {
                // Manejar errores que puedan ocurrir durante la consulta
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
