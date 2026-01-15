using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Models;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    int? ValidateToken(string token);
}
