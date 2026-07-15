using Memo.Domain.Entities;
using Memo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Memo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MemoDbContext _context;

        public UserRepository(MemoDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task AddAsync(UserEntity entity)
            => await _context.Users.AddAsync(entity);

        public void Update(UserEntity entity)
            => _context.Users.Update(entity);

        public void Delete(UserEntity entity)
            => _context.Users.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<UserEntity?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> IsEmailUniqueAsync(string email)
            => !await _context.Users.AnyAsync(u => u.Email == email);
    }
}