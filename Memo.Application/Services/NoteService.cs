using Memo.Application.DTOs.Notes;
using Memo.Application.Interfaces;
using Memo.Domain.Entities;
using Memo.Domain.Interfaces;

namespace Memo.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;

        public NoteService(INoteRepository noteRepository, IUserRepository userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }

        public async Task<NoteEntity> CreateNoteAsync(CreateNoteDto request, int? userId = null)
        {
            var expiresAt = request.Lifetime switch
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
                Content = request.Content,
                UserId = userId,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow,
                IsBurnAfterReading = request.IsBurnAfterReading  // <-- ДОБАВИТЬ
            };

            await _noteRepository.AddAsync(note);
            await _noteRepository.SaveChangesAsync();

            return note;
        }

        public async Task<NoteEntity?> GetNoteByShortCodeAsync(string shortCode)
        {
            var note = await _noteRepository.GetByShortCodeAsync(shortCode);
            if (note == null || note.IsDeleted)
                return null;

            // Проверка истечения
            if (note.ExpiresAt.HasValue && note.ExpiresAt.Value < DateTime.UtcNow)
            {
                note.IsDeleted = true;
                await _noteRepository.SaveChangesAsync();
                return null;
            }

            // 🔥 Сожжение после прочтения
            if (note.IsBurnAfterReading)
            {
                note.IsDeleted = true;
                await _noteRepository.SaveChangesAsync();
            }

            return note;
        }

        public async Task<IEnumerable<NoteResponseDto>> GetUserNotesAsync(int userId)
        {
            var notes = await _noteRepository.GetUserNotesAsync(userId);
            return notes.Select(n => new NoteResponseDto
            {
                ShortCode = n.ShortCode,
                Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                ExpiresAt = n.ExpiresAt,
                CreatedAt = n.CreatedAt,
                IsExpired = n.ExpiresAt.HasValue && n.ExpiresAt.Value < DateTime.UtcNow
            });
        }

        public async Task<bool> DeleteNoteAsync(string shortCode, int userId)
        {
            var note = await _noteRepository.GetByShortCodeAsync(shortCode);
            if (note == null || note.UserId != userId)
                return false;

            note.IsDeleted = true;
            await _noteRepository.SaveChangesAsync();
            return true;
        }

        public async Task CleanupExpiredNotesAsync()
        {
            await _noteRepository.DeleteExpiredNotesAsync();
        }

        public async Task<string> GenerateUniqueShortCodeAsync()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            int maxAttempts = 10;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                var code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                if (await _noteRepository.IsShortCodeUniqueAsync(code))
                    return code;
            }

            throw new InvalidOperationException("Failed to generate unique short code");
        }

        public async Task<bool> UpdateNoteAsync(string shortCode, int userId, string newContent)
        {
            var note = await _noteRepository.GetByShortCodeAsync(shortCode, true);
            if (note == null || note.UserId != userId || note.IsDeleted)
                return false;

            // Проверяем, не истекла ли заметка
            if (note.ExpiresAt.HasValue && note.ExpiresAt.Value < DateTime.UtcNow)
                return false;

            note.Content = newContent;
            note.UpdatedAt = DateTime.UtcNow; // Добавь это поле в NoteEntity
            await _noteRepository.SaveChangesAsync();
            return true;
        }
    }
}