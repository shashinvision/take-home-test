using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Fundo.Applications.WebApi.DTOs;
using System;
using System.Text.Json;
using System.Linq;

namespace Fundo.Applications.WebApi.Controllers
{

    // [Authorize]
    [Route("/loans")]
    public class LoanManagementController : Controller
    {
        private readonly ILoanManagementService _loanManagementService;

        public LoanManagementController(ILoanManagementService loanManagementService)
        {
            _loanManagementService = loanManagementService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var loans = await _loanManagementService.GetAllLoans();
            return Ok(loans);
        }

        [HttpGet("/applicants")]
        public async Task<ActionResult> GetAllApplicantsLoans()
        {
            var applicants = await _loanManagementService.GetAllApplicantsLoans();
            return Ok(new
            {
                count = applicants.Count(),
                data = applicants
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var loan = await _loanManagementService.GetLoanById(id);
            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult> CreateLoan([FromBody] LoanDto loanDto)
        {
            try
            {
                Console.WriteLine(
                    JsonSerializer.Serialize(loanDto)
                );

                if (loanDto == null)
                    return BadRequest("Loan data is required");

                if (loanDto.Amount == null || loanDto.Amount <= 0)
                    return BadRequest("Amount must be greater than 0");

                if (loanDto.IdApplicant == null || loanDto.IdApplicant <= 0)
                    return BadRequest("Valid applicant ID is required");

                await _loanManagementService.CreateLoan(loanDto);

                return Ok(new
                {
                    message = "Loan created successfully",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
