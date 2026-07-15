using Memo.Application.DTOs.Notes;

namespace Memo.Application.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<NoteResponseDto>> SearchNotesAsync(
            int userId,
            NoteSearchDto searchDto
        );
    }
}