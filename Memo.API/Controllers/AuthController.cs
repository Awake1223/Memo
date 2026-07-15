using Microsoft.AspNetCore.Mvc;
using Memo.Application.DTOs.Auth;
using Memo.Application.Interfaces;

namespace Memo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;  // Используй интерфейс

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Передаём ВЕСЬ DTO, а не отдельные поля
            var user = await _authService.RegisterAsync(request);
            if (user == null)
                return BadRequest(new { message = "User with this email already exists" });

            return Ok(new { message = "Registration successful", userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Передаём ВЕСЬ DTO, а не отдельные поля
            var token = await _authService.LoginAsync(request);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var user = await _authService.GetUserByEmailAsync(request.Email);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = request.Email,
                UserId = user?.Id ?? 0,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}