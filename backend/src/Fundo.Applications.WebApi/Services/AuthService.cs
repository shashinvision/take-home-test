using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Infraestructure.Repositories;
using System.Collections.Generic;
using System;

namespace Fundo.Applications.WebApi.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IBaseRepository<User> _baseUserRepository;
    private readonly IAuthRepository<User> _authUserRepository;


    public AuthService(ITokenService tokenService, IBaseRepository<User> baseUserRepository, IAuthRepository<User> authUserRepository)
    {
        _tokenService = tokenService;
        _baseUserRepository = baseUserRepository;
        _authUserRepository = authUserRepository;
    }


    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        User user = await _authUserRepository.GetByEmail(email: request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        if (!VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Name = user.FullName
            }
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {

        User existingUser = await _authUserRepository.GetByEmail(email: request.Email);

        if (existingUser != null)
            throw new InvalidOperationException("User already exists");

        var user = new User
        {
            Email = request.Email,
            FullName = request.Name,
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = 1
        };


        await _baseUserRepository.Add(user);

        var token = _tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Name = user.FullName
            }
        };
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _authUserRepository.GetByEmail(email: email);

    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
