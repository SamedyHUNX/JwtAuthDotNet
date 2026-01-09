using JwtAuthDotNet.Dtos;
using JwtAuthDotNet.Entities;
using JwtAuthDotNet.Models;

namespace JwtAuthDotNet.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
}
