using Microsoft.AspNetCore.Mvc;
using Memo.API.DTOs.Auth;
using Memo.API.Services;

namespace Memo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.RegisterAsync(request.Email, request.Password);
            if (user == null)
                return BadRequest(new { message = "User with this email already exists" });

            return Ok(new { message = "Registration successful", userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new AuthResponse
            {
                Token = token,
                Email = request.Email,
                UserId = await GetUserIdByEmail(request.Email),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
        }

        private async Task<int> GetUserIdByEmail(string email)
        {
            var user = await _authService.GetUserByEmailAsync(email);
            return user?.Id ?? 0;
        }
    }
}