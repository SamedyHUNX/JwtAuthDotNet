using JwtAuthDotNet.Dtos;
using JwtAuthDotNet.Entities;

namespace JwtAuthDotNet.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<string?> LoginAsync(UserDto request);
}
