using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Infraestructure.Repositories;

public interface IAuthRepository<User> where User : class
{
    Task<User> GetByEmail(string email);
}
