using System;
using Memo.Domain.Entities;
using Memo.Domain;
using Microsoft.EntityFrameworkCore;
using Memo.Domain.Interfaces;

namespace Memo.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly MemoDbContext _context;

        public NoteRepository(MemoDbContext context)
        {
            _context = context;
        }

        public async Task<NoteEntity?> GetByIdAsync(int id)
            => await _context.Notes.FindAsync(id);

        public async Task<IEnumerable<NoteEntity>> GetAllAsync()
            => await _context.Notes.Where(n => !n.IsDeleted).ToListAsync();

        public async Task AddAsync(NoteEntity entity)
            => await _context.Notes.AddAsync(entity);

        public void Update(NoteEntity entity)
            => _context.Notes.Update(entity);

        public void Delete(NoteEntity entity)
            => _context.Notes.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<NoteEntity?> GetByShortCodeAsync(string shortCode)
            => await _context.Notes
                .FirstOrDefaultAsync(n => n.ShortCode == shortCode && !n.IsDeleted);

        public async Task<IEnumerable<NoteEntity>> GetUserNotesAsync(int userId)
            => await _context.Notes
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public async Task<bool> IsShortCodeUniqueAsync(string shortCode)
            => !await _context.Notes.AnyAsync(n => n.ShortCode == shortCode);

        public async Task DeleteExpiredNotesAsync()
        {
            var expired = await _context.Notes
                .Where(n => n.ExpiresAt != null && n.ExpiresAt < DateTime.UtcNow && !n.IsDeleted)
                .ToListAsync();

            foreach (var note in expired)
            {
                note.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<NoteEntity?> GetByShortCodeAsync(string shortCode, bool includeDeleted = false)
        {
            var query = _context.Notes.AsQueryable();
            if (!includeDeleted)
                query = query.Where(n => !n.IsDeleted);

            return await query.FirstOrDefaultAsync(n => n.ShortCode == shortCode);
        }
    }
}