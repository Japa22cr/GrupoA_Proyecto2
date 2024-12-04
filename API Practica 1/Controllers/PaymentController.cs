﻿  using DataAccess.EF.Models;
using DataAccess.EF;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using BL.IServices;
using BL.Services;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        private readonly ClaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public PaymentController(ClaseDbContext context, UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // Pay Fine 
        [HttpPost]
        public async Task<IActionResult> PayFine([FromBody] PaymentRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
 
                // Retrieve the fine from the database
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

                // Create the payment
                var payment = new Payment
                {
                    FineId = fine.Id,
                    UserId = user.Id,
                    Amount = model.Amount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentDate = DateTime.UtcNow
                };

                _context.Payments.Add(payment);

                // Update fine status
                fine.PaymentId = payment.Id;

                fine.Estado = true;

                await _context.SaveChangesAsync();

                // Send confirmation email
                //await _emailService.SendConfirmationEmailAsync(user.UserEmail, fine, payment);

                return Ok(new { Message = "Fine paid successfully." });
            }
            catch (Exception ex)
            {
                // Handle exception and log
                return StatusCode(500, "An error occurred while processing the payment.");
            }
        }

        // Get Payments by user. 

    }
}
