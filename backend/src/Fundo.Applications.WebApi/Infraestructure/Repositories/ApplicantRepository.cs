using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public class ApplicantRepository : IBaseRepository<Applicant>
{
    private readonly FundoDbContext _context;

    public ApplicantRepository(FundoDbContext context)
    {
        _context = context;
    }

    public async Task<Applicant> GetById(int id)
    {
        return await _context.Applicants.FindAsync(id);
    }

    public async Task<IEnumerable<Applicant>> GetAll()
    {
        return await _context.Applicants
            .ToListAsync();
    }

    public async Task Add(Applicant applicant)
    {
        await _context.Applicants.AddAsync(applicant);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Applicant payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var applicant = await _context.Applicants.FindAsync(id);
        if (applicant != null)
        {
            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();
        }
    }
}
