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
        [HttpPost("dispute-fine")]
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

                // Send confirmation email of the dispute to the user
                await _emailService.SendDisputeConfirmationEmail(user.Email, dispute, fine);

                return Ok(new { Message = "Fine disputed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the dispute.");
            }
        }

        // Handle Dispute
        [HttpPost("handle-dispute")]
        public async Task<IActionResult> HandleDispute([FromBody] HandleDisputeDto model)
        {

            try { 

                var dispute = await _context.Disputes
                   .Include(d => d.Fine)
                   .Include(d => d.User)
                   .FirstOrDefaultAsync(d => d.Id == model.DisputeId);

                if (dispute == null)
                {
                    throw new ArgumentException("Dispute not found.");
                }

                if (dispute.IsResolved)
                {
                    throw new InvalidOperationException("Dispute is already resolved.");
                }

                // Update the dispute properties
                dispute.Resolution = model.Resolution;
                dispute.ResolutionDate = DateTime.UtcNow;
                dispute.IsResolved = true;
                dispute.JudgeId = model.JudgeId;

                // Update the database
                _context.Disputes.Update(dispute);
                await _context.SaveChangesAsync();

                // Send confirmation email of the dispute resolution officer
                await _emailService.SendDisputeResolutionOfficer(dispute.User.UserName, dispute);

                // Send confirmation email of the dispute resolution user
                await _emailService.SendDisputeResolutionUser(dispute.User.UserName, dispute);

                return Ok(new { Message = "Fine disputed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the dispute.");
            }

        }

        // Get Disputes by user
        [HttpGet]
        public async Task<IActionResult> GetDisputesUser(string userin)
        {
            // Validar el parámetro de correo electrónico
            if (string.IsNullOrEmpty(userin))
            {
                return BadRequest("El usuario es requerido.");
            }

            // Buscar al usuario por correo electrónico
            var user = await _userManager.FindByNameAsync(userin);
            if (user == null)
            {
                return NotFound("No se ha encontrado un usuario.");
            }

            try
            {
                // Obtener todas las multas asociadas al usuario
                var disputes = await _context.Disputes
                    .Select(d => new
                    {
                        d.Id,
                        Fine = new
                        {
                            d.Fine.Id,
                            d.Fine.Category,
                            d.Fine.Article,
                            d.Fine.Conduct,
                            d.Fine.Estado,
                            d.Fine.Amount,
                        },
                        d.CreatedDate,
                        d.Reason,
                        User = new
                        {
                            d.User.Id,
                            d.User.UserName,
                            d.User.Email
                        },
                        d.IsResolved,
                        Judge = new
                        {
                            d.Judge.Id,
                            d.Judge.UserName,
                            d.Judge.Email
                        },
                        d.Resolution,
                        d.ResolutionDate
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

        // Get All Disputes
        [HttpGet("get-all-disputes")]
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
                        User = new
                        {
                            d.User.Id,
                            d.User.UserName,
                            d.User.Email
                        },
                        d.IsResolved,
                        Judge = new
                        {
                            d.Judge.Id,
                            d.Judge.UserName,
                            d.Judge.Email 
                        },
                        d.Resolution,
                        d.ResolutionDate
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
