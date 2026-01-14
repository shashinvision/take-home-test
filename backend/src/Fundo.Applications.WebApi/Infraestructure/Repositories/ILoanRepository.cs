using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public interface IloanRepository
{
    Task<Loan> GetById(int id);
    Task<IEnumerable<Loan>> GetAll();
    Task Add(Loan loan);
    Task Update(Loan loan);
    Task Delete(int id);
}
