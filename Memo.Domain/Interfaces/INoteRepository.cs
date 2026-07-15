using Memo.Domain.Entities;

namespace Memo.Domain.Interfaces
{
    public interface INoteRepository : IRepository<NoteEntity>
    {
        Task<NoteEntity?> GetByShortCodeAsync(string shortCode);
        Task<IEnumerable<NoteEntity>> GetUserNotesAsync(int userId);
        Task<bool> IsShortCodeUniqueAsync(string shortCode);
        Task DeleteExpiredNotesAsync();

        Task<NoteEntity?> GetByShortCodeAsync(string shortCode, bool includeDeleted = false);

        Task<IEnumerable<NoteEntity>> SearchNotesAsync(
            int userId,
            string? query,
            string? tag,
            int page,
            int pageSize,
            string sortBy
        );

        Task<IEnumerable<NoteEntity>> GetTrendingNotesAsync(int limit);
    }
}