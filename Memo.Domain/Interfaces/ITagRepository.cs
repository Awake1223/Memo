using Memo.Domain.Entities;

namespace Memo.Domain.Interfaces
{
    public interface ITagRepository : IRepository<TagEntity>
    {
        Task<TagEntity?> GetByNameAsync(string name);
        Task<IEnumerable<TagEntity>> GetPopularTagsAsync(int limit);
    }
}