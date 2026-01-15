using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services;

public interface ILoanManagementService
{
    Task<IEnumerable<Loan>> GetAllLoans();
    Task<Loan> GetLoanById(int id);
    Task CreateLoan(LoanDto loanDto);
    Task UpdateLoan(Loan loan);
    Task DeleteLoan(int id);
}
