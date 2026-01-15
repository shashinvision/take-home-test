using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Services;

namespace Fundo.Applications.WebApi.Controllers
{
    [Route("/loans")]
    public class LoanManagementController : Controller
    {
        private readonly LoanManagementService _loanManagementService;

        public LoanManagementController(LoanManagementService loanManagementService)
        {
            _loanManagementService = loanManagementService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var loans = await _loanManagementService.GetAllLoans();
            return Ok(loans);
        }
    }
}
