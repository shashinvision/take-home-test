using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.Infraestructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.DTOs;
using System;

namespace Fundo.Applications.WebApi.Services;

public class LoanManagementService : ILoanManagementService
{
    private readonly IBaseRepository<Loan> _loanRepository;

    public LoanManagementService(IBaseRepository<Loan> loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<Loan>> GetAllLoans()
    {
        return await _loanRepository.GetAll();
    }

    public async Task<Loan> GetLoanById(int id)
    {
        return await _loanRepository.GetById(id);
    }

    public async Task CreateLoan(LoanDto loanDto)
    {


        var loan = new Loan
        {
            Amount = loanDto.Amount,
            CurrentBalance = loanDto.CurrentBalance,
            IsActive = loanDto.IsActive,
            CreatedAt = DateTime.Now,
            UpdateAt = DateTime.Now,
            IdApplicant = loanDto.IdApplicant,

        };

        await _loanRepository.Add(loan);
    }

    public async Task UpdateLoan(Loan loan)
    {
        await _loanRepository.Update(loan);
    }

    public async Task DeleteLoan(int id)
    {
        await _loanRepository.Delete(id);
    }
}
