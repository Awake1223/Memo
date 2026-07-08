using Microsoft.EntityFrameworkCore;
using Memo.API.Data;
using Memo.API.Data.Entities;
using Memo.API.DTOs.Notes;

namespace Memo.API.Services
{
    public class NoteService
    {
        private readonly MemoDbContext _context;
        private readonly ILogger<NoteService> _logger;

        public NoteService(MemoDbContext context, ILogger<NoteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<NoteEntity?> CreateNoteAsync(string content, NoteLifetime lifetime, int? userId = null)
        {
            var expiresAt = lifetime switch
            {
                NoteLifetime.OneHour => DateTime.UtcNow.AddHours(1),
                NoteLifetime.OneDay => DateTime.UtcNow.AddDays(1),
                NoteLifetime.Forever => (DateTime?)null,
                _ => null
            };

            var shortCode = await GenerateUniqueShortCodeAsync();

            var note = new NoteEntity
            {
                ShortCode = shortCode,
                Content = content,
                UserId = userId,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<NoteEntity?> GetNoteByShortCodeAsync(string shortCode)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.ShortCode == shortCode && !n.IsDeleted);

            if (note == null)
                return null;

            // Проверяем, не истекла ли заметка
            if (note.ExpiresAt.HasValue && note.ExpiresAt.Value < DateTime.UtcNow)
            {
                note.IsDeleted = true;
                await _context.SaveChangesAsync();
                return null;
            }

            return note;
        }

        public async Task<List<NoteEntity>> GetUserNotesAsync(int userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteNoteAsync(string shortCode, int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.ShortCode == shortCode && n.UserId == userId && !n.IsDeleted);

            if (note == null)
                return false;

            note.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> GenerateUniqueShortCodeAsync()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            int maxAttempts = 10;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                var code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var exists = await _context.Notes.AnyAsync(n => n.ShortCode == code);
                if (!exists)
                    return code;
            }

            // Если не удалось сгенерировать уникальный код за 10 попыток
            throw new Exception("Failed to generate unique short code");
        }
    }
}