using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public class LoanRepository : IBaseRepository<Loan>
{
    private readonly FundoDbContext _context;

    public LoanRepository(FundoDbContext context)
    {
        _context = context;
    }

    public async Task<Loan> GetById(int id)
    {
        return await _context.Loans.FindAsync(id);
    }

    public async Task<IEnumerable<Loan>> GetAll()
    {
        return await _context.Loans
            .Include(l => l.IdApplicantNavigation)
            .Include(l => l.Payments)
            .ToListAsync();
    }

    public async Task Add(Loan loan)
    {
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Loan loan)
    {
        _context.Entry(loan).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan != null)
        {
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}
