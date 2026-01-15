using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public class PaymentRepository : IBaseRepository<Payment>
{
    private readonly FundoDbContext _context;

    public PaymentRepository(FundoDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> GetById(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAll()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task Add(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Payment payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
