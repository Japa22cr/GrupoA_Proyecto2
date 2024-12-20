﻿using DataAccess.EF.Models;
using DataAccess.EF;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BL.Services;
using BL.IServices;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinesController : ControllerBase
    {
        private readonly ClaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public FinesController(ClaseDbContext context, UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // Add Fine
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
                Inspector = model.Inspector,
                IssuedDate = DateTime.UtcNow,
                UserId = user.Id, // Associate fine with the found user's Id
                Place = model.Place,
                LicensePlate = model.LicensePlate,
                Category = model.Category,
                Article = model.Article,
                Description = model.Description,
                Conduct = model.Conduct,
                Amount = model.Amount
            };

            try
            {
                // Add the Fine to the database
                _context.Fines.Add(fine);
                await _context.SaveChangesAsync();

                // Send confirmation email for the officer
                await _emailService.SendFineConfirmationEmailOfficer(user.Email, fine);

                // Send fine alert to the user
                await _emailService.SendFineAlertEmailUser(user.Email, fine);

                return Ok("Fine added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during database save
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Get Fines by user
        [HttpGet]
        public async Task<IActionResult> GetFinesByUser(string userin)
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
                var fines = await _context.Fines
                    .Where(f => f.UserId == user.Id)
                    .Select(f => new
                    {
                        f.Id,
                        f.IssuedDate,
                        f.Inspector,
                        f.Place,
                        f.LicensePlate,
                        f.Category,
                        f.Article,
                        f.Description,
                        f.Conduct,
                        f.Amount,
                        f.Estado
                    })
                    .ToListAsync();

                // Verificar si el usuario tiene multas
                if (!fines.Any())
                {
                    return NotFound("No se encuentra ninguna multa.");
                }

                return Ok(fines);
            }
            catch (Exception ex)
            {
                // Manejar errores que puedan ocurrir durante la consulta
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Get All Fines 
        [HttpGet("get-all-fines")]
        public async Task<IActionResult> GetAllFines()
        {
            try
            {
                // Obtener todas las multas asociadas al usuario
                var fines = await _context.Fines
                    .Select(f => new
                    {
                        f.Id,
                        f.IssuedDate,
                        f.Inspector,
                        f.Place,
                        f.LicensePlate,
                        f.Category,
                        f.Article,
                        f.Description,
                        f.Conduct,
                        f.Amount,
                        f.Estado
                    })
                    .ToListAsync();

                // Verificar si el usuario tiene multas
                if (!fines.Any())
                {
                    return NotFound("No se encuentra ninguna multa.");
                }

                return Ok(fines);
            }
            catch (Exception ex)
            {
                // Manejar errores que puedan ocurrir durante la consulta
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
