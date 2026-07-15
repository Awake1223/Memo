using Memo.Domain.Entities;

namespace Memo.Domain.Interfaces
{
    public interface INoteRepository : IRepository<NoteEntity>
    {
        Task<NoteEntity?> GetByShortCodeAsync(string shortCode);
        Task<IEnumerable<NoteEntity>> GetUserNotesAsync(int userId);
        Task<bool> IsShortCodeUniqueAsync(string shortCode);
        Task DeleteExpiredNotesAsync();
    }
}