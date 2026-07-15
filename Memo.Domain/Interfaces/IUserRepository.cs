using Memo.Domain.Entities;

namespace Memo.Domain.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}