namespace Memo.Application.DTOs.Notes
{
    public class NoteSearchDto
    {
        public string? Query { get; set; }      // текст поиска
        public string? Tag { get; set; }         // фильтр по тегу
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "relevance"; // relevance, newest, popular
    }
}