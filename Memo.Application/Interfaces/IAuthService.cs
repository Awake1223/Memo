using Memo.Application.DTOs.Auth;
using Memo.Domain.Entities;

namespace Memo.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserEntity?> GetUserByIdAsync(int id);
        Task<string?> LoginAsync(LoginDto request);
        Task<UserEntity?> RegisterAsync(RegisterDto request);
    }
}