using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Memo.Application.DTOs.Auth;
using Memo.Application.Interfaces;
using Memo.Domain.Entities;
using Memo.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;



namespace Memo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<UserEntity?> RegisterAsync(RegisterDto request)
        {
            if (!await _userRepository.IsEmailUniqueAsync(request.Email))
                return null;

            var user = new UserEntity
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<string?> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
            => await _userRepository.GetByIdAsync(id);

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
            => await _userRepository.GetByEmailAsync(email);

        private string GenerateJwtToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LrMC3k1hXknl8brbdc7jCmLhRwfrPEnaIceyeIa59No"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}