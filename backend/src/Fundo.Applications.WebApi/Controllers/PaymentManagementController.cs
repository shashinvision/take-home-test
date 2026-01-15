using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Services;
using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using System;
using System.Text.Json;
using System.Runtime.ExceptionServices;


namespace Fundo.Applications.WebApi.Controllers
{
    [Route("/payment")]
    public class PaymentManagementController : Controller
    {
        private readonly PaymentManagementService _paymentManagementService;

        public PaymentManagementController(PaymentManagementService paymentManagementService)
        {
            _paymentManagementService = paymentManagementService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                Console.WriteLine(
                    JsonSerializer.Serialize(paymentDto)
                );

                if (paymentDto == null)
                    return BadRequest("Payment data is required");

                if (paymentDto.Amount == null || paymentDto.Amount <= 0)
                    return BadRequest("Amount must be greater than 0");

                if (paymentDto.IdLoan <= 0)
                    return BadRequest("Valid id loan ID is required");

                await _paymentManagementService.CreatePayment(paymentDto);

                return Ok(new
                {
                    message = "Payment created successfully",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
