using JwtAuthDotNet.Dtos;
using JwtAuthDotNet.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDotNet.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();

        [HttpPost("register")]
        public ActionResult Register(UserDto request)
        {
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, request.Password);
            
            // Store the hashed password
            user.PasswordHash = hashedPassword;
            
            return Ok(user);
        }
    }
}
