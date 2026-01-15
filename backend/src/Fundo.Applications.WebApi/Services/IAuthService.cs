using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Models;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services;


public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<User?> GetUserByEmailAsync(string email);
}
