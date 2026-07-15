using Memo.Domain.Entities;
using Memo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Memo.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MemoDbContext _context;

        public TagRepository(MemoDbContext context)
        {
            _context = context;
        }

        public async Task<TagEntity?> GetByIdAsync(int id)
            => await _context.Tags.FindAsync(id);

        public async Task<IEnumerable<TagEntity>> GetAllAsync()
            => await _context.Tags.ToListAsync();

        public async Task AddAsync(TagEntity entity)
            => await _context.Tags.AddAsync(entity);

        public void Update(TagEntity entity)
            => _context.Tags.Update(entity);

        public void Delete(TagEntity entity)
            => _context.Tags.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<TagEntity?> GetByNameAsync(string name)
            => await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name);

        public async Task<IEnumerable<TagEntity>> GetPopularTagsAsync(int limit)
        {
            return await _context.Tags
            .Include(t => t.NoteTags)  // <-- ДОБАВИТЬ Include
            .OrderByDescending(t => t.NoteTags.Count)
            .Take(limit)
            .ToListAsync();
        }
    }
}