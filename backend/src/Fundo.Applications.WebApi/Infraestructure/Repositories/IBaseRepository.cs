using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(int id);
}
