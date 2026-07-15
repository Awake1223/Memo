
namespace Memo.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity entity);
        void Delete(UserEntity entity);
        Task<IEnumerable<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity?> GetByIdAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email);
        Task SaveChangesAsync();
        void Update(UserEntity entity);
    }
}