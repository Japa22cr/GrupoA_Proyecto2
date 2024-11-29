
﻿using DataAccess.EF.Models;
using DataAccess.EF;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        //  POST method for processing a payment
        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            // Validate the input DTO
            if (paymentRequest == null || paymentRequest.Amount <= 0)
            {
                return BadRequest("Invalid payment request. Please check the details and try again.");
            }

            // Simulate payment processing
            var paymentResponse = new PaymentResponseDto
            {
                PaymentId = paymentRequest.PaymentId,
                Status = "Success",     // successful payment status
                TransactionId = Guid.NewGuid().ToString(), // Generate a unique transaction ID
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                Message = "Payment processed successfully.",
                ProcessedDate = DateTime.UtcNow
            };

            // Return the response DTO
            return Ok(paymentResponse);
        }
    }
}
