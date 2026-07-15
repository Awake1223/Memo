using Memo.Application.DTOs.Notes;
using Memo.Domain.Entities;

namespace Memo.Application.Interfaces
{
    public interface INoteService
    {
        Task CleanupExpiredNotesAsync();
        Task<NoteEntity> CreateNoteAsync(string content, NoteLifetime lifetime, int? userId = null);
        Task<bool> DeleteNoteAsync(string shortCode, int userId);
        Task<string> GenerateUniqueShortCodeAsync();
        Task<NoteEntity?> GetNoteByShortCodeAsync(string shortCode);
        Task<IEnumerable<NoteResponseDto>> GetUserNotesAsync(int userId);
    }
}