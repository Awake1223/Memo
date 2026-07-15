using Memo.Application.DTOs.Notes;
using Memo.Application.Interfaces;
using Memo.Domain.Interfaces;

namespace Memo.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly INoteRepository _noteRepository;

        public SearchService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<NoteResponseDto>> SearchNotesAsync(
            int userId,
            NoteSearchDto searchDto)
        {
            var notes = await _noteRepository.SearchNotesAsync(
                userId,
                searchDto.Query,
                searchDto.Tag,
                searchDto.Page,
                searchDto.PageSize,
                searchDto.SortBy
            );

            return notes.Select(n => new NoteResponseDto
            {
                ShortCode = n.ShortCode,
                Content = n.Content.Length > 200 ? n.Content[..200] + "..." : n.Content,
                ExpiresAt = n.ExpiresAt,
                CreatedAt = n.CreatedAt,
                IsExpired = n.ExpiresAt.HasValue && n.ExpiresAt.Value < DateTime.UtcNow,
                ViewCount = n.ViewCount
            });
        }
    }
}