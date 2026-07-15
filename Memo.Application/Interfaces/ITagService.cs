using Memo.Application.DTOs.Tags;
using Memo.Domain.Entities;

namespace Memo.Application.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetPopularTagsAsync(int limit = 10);
        Task<TagEntity?> GetOrCreateTagAsync(string tagName);
        Task ExtractAndSaveTagsAsync(NoteEntity note, string content);
    }
}