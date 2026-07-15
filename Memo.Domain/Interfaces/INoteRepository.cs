using Memo.API.Data.Entities;

namespace Memo.Infrastructure.Repositories
{
    public interface INoteRepository
    {
        Task AddAsync(NoteEntity entity);
        void Delete(NoteEntity entity);
        Task DeleteExpiredNotesAsync();
        Task<IEnumerable<NoteEntity>> GetAllAsync();
        Task<NoteEntity?> GetByIdAsync(int id);
        Task<NoteEntity?> GetByShortCodeAsync(string shortCode);
        Task<IEnumerable<NoteEntity>> GetUserNotesAsync(int userId);
        Task<bool> IsShortCodeUniqueAsync(string shortCode);
        Task SaveChangesAsync();
        void Update(NoteEntity entity);
    }
}