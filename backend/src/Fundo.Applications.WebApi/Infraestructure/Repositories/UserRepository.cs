using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public class UserRepository : IBaseRepository<User>, IAuthRepository<User>
{
    private readonly FundoDbContext _context;

    public UserRepository(FundoDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive == 1);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var payment = await _context.Users.FindAsync(id);
        if (payment != null)
        {
            _context.Users.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
