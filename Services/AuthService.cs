using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtAuthDotNet.Data;
using JwtAuthDotNet.Dtos;
using JwtAuthDotNet.Entities;
using JwtAuthDotNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthDotNet.Services;

public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
{
    #region Public Methods

    public async Task<User?> RegisterAsync(UserDto request)
    {
        if (await UserExistsAsync(request.Username))
        {
            return null;
        }

        var user = CreateUserFromRequest(request);

        // Keep tracking
        context.Users.Add(user);
        // Save the user to the database
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !VerifyPassword(user, request.Password))
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

        if (user is null)
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }

    #endregion

    #region Private Helper Methods - User Operations

    private async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }

    private User CreateUserFromRequest(UserDto request)
    {
        var user = new User { Username = request.Username };
        user.PasswordHash = HashPassword(user, request.Password);
        return user;
    }

    private string HashPassword(User user, string password)
    {
        return new PasswordHasher<User>().HashPassword(user, password);
    }

    private bool VerifyPassword(User user, string password)
    {
        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        return result != PasswordVerificationResult.Failed;
    }

    #endregion

    #region Private Helper Methods - Token Operations

    private string CreateToken(User user)
    {
        // Claims: what you want to include in the token payload
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };

        // Get secret key from configuration (configuration injection)
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!)
        );

        // Signing credentials
        // Length 64 bytes (characters): 512 bits
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        // Token descriptor
        var tokenDescriptor = new JwtSecurityToken
        (
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync();
        return refreshToken;
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }

    private async Task<TokenResponseDto> CreateTokenResponse(User user)
    {
        return new TokenResponseDto
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    #endregion
}